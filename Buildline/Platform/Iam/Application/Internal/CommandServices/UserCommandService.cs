using Buildline.Platform.Iam.Application.CommandServices;
using Buildline.Platform.Iam.Application.Internal.OutboundServices;
using Buildline.Platform.Iam.Domain.Model;
using Buildline.Platform.Iam.Domain.Model.Aggregates;
using Buildline.Platform.Iam.Domain.Model.Commands;
using Buildline.Platform.Iam.Domain.Repositories;
using Buildline.Platform.Resources.Errors;
using Buildline.Platform.Shared.Application.Model;
using Buildline.Platform.Shared.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Buildline.Platform.Iam.Application.Internal.CommandServices;

/// <summary>
///     Application command service that coordinates IAM writes for authentication and user management.
/// </summary>
/// <remarks>
///     The service follows the course sample architecture: controllers translate HTTP payloads into
///     commands, this service enforces application rules, repositories load or persist aggregates,
///     and <see cref="IUnitOfWork"/> commits the transaction. It returns <see cref="Result{T}"/>
///     instead of throwing business exceptions so REST assemblers can produce consistent Problem
///     Details responses.
/// </remarks>
public class UserCommandService(
    IUserRepository userRepository,
    IHashingService hashingService,
    ITokenService tokenService,
    IUnitOfWork unitOfWork,
    IStringLocalizer<ErrorMessages> localizer)
    : IUserCommandService
{
    private static readonly string[] SupportedRoles = ["owner", "admin", "viewer"];

    private static bool IsSupportedRole(string role) =>
        SupportedRoles.Any(supportedRole => string.Equals(supportedRole, role, StringComparison.OrdinalIgnoreCase));

    private Result<User>? ValidateManagedRole(string requestedRole, string? currentRole = null)
    {
        if (!IsSupportedRole(requestedRole))
            return Result<User>.Failure(
                IamError.InvalidRole,
                localizer[$"{nameof(IamError)}.{IamError.InvalidRole}"]);

        if (string.Equals(requestedRole, "owner", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(currentRole, "owner", StringComparison.OrdinalIgnoreCase))
            return Result<User>.Failure(
                IamError.InvalidRole,
                localizer[$"{nameof(IamError)}.{IamError.InvalidRole}"]);

        if (string.Equals(currentRole, "owner", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(requestedRole, "owner", StringComparison.OrdinalIgnoreCase))
            return Result<User>.Failure(
                IamError.InvalidRole,
                localizer[$"{nameof(IamError)}.{IamError.InvalidRole}"]);

        return null;
    }

    /// <summary>
    ///     Handles user creation from the administration module.
    /// </summary>
    /// <param name="command">Create-user command built from the REST resource.</param>
    /// <param name="cancellationToken">Token used to cancel repository and unit-of-work work.</param>
    /// <returns>
    ///     A successful result with the persisted user, or an IAM error for duplicated email,
    ///     cancellation, database failure or unexpected server failure.
    /// </returns>
    /// <remarks>
    ///     The plain password is hashed before the aggregate is constructed. This keeps credential
    ///     hashing outside the domain object while ensuring no plain credential crosses the
    ///     persistence boundary.
    /// </remarks>
    public async Task<Result<User>> Handle(CreateUserCommand command, CancellationToken cancellationToken = default)
    {
        var roleValidation = ValidateManagedRole(command.Role);
        if (roleValidation is not null) return roleValidation;

        if (string.IsNullOrWhiteSpace(command.Password) || command.Password.Length < 6)
            return Result<User>.Failure(
                IamError.WeakPassword,
                localizer[$"{nameof(IamError)}.{IamError.WeakPassword}"]);

        if (await userRepository.ExistsByEmailAsync(command.Email, cancellationToken))
            return Result<User>.Failure(
                IamError.EmailAlreadyTaken,
                localizer[$"{nameof(IamError)}.{IamError.EmailAlreadyTaken}"]);

        var user = new User(
            command.Name,
            command.Email,
            hashingService.HashPassword(command.Password),
            command.Role,
            command.Department,
            command.Phone,
            command.AvatarColor,
            command.IsActive,
            command.LastLogin);

        try
        {
            await userRepository.AddAsync(user, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<User>.Success(user);
        }
        catch (OperationCanceledException)
        {
            return Result<User>.Failure(
                IamError.OperationCancelled,
                localizer[$"{nameof(IamError)}.{IamError.OperationCancelled}"]);
        }
        catch (DbUpdateException)
        {
            return Result<User>.Failure(
                IamError.DatabaseError,
                localizer[$"{nameof(IamError)}.{IamError.DatabaseError}"]);
        }
        catch (Exception)
        {
            return Result<User>.Failure(
                IamError.InternalServerError,
                localizer[$"{nameof(IamError)}.{IamError.InternalServerError}"]);
        }
    }

    /// <summary>
    ///     Handles administrative updates over an existing IAM user account.
    /// </summary>
    /// <param name="command">Update command containing the user id and the complete replacement values.</param>
    /// <param name="cancellationToken">Token used to cancel lookup, uniqueness validation and persistence.</param>
    /// <returns>
    ///     A successful result with the updated user, or an IAM error when the user does not exist,
    ///     the email belongs to another account, persistence fails, or the operation is cancelled.
    /// </returns>
    /// <remarks>
    ///     The method validates email uniqueness before mutating the aggregate. Password hash and
    ///     last-login are deliberately untouched because US-024 only covers role, status and account
    ///     metadata management from the frontend users module.
    /// </remarks>
    public async Task<Result<User>> Handle(UpdateUserCommand command, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.FindByIdAsync(command.UserId, cancellationToken);
        if (user is null)
            return Result<User>.Failure(
                IamError.UserNotFound,
                localizer[$"{nameof(IamError)}.{IamError.UserNotFound}"]);

        var roleValidation = ValidateManagedRole(command.Role, user.Role);
        if (roleValidation is not null) return roleValidation;

        var userWithSameEmail = await userRepository.FindByEmailAsync(command.Email, cancellationToken);
        if (userWithSameEmail is not null && userWithSameEmail.Id != command.UserId)
            return Result<User>.Failure(
                IamError.EmailAlreadyTaken,
                localizer[$"{nameof(IamError)}.{IamError.EmailAlreadyTaken}"]);

        try
        {
            user.UpdateAccountInformation(
                command.Name,
                command.Email,
                command.Role,
                command.Department,
                command.Phone,
                command.AvatarColor,
                command.IsActive,
                command.TwoFactorEnabled);
            userRepository.Update(user);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<User>.Success(user);
        }
        catch (OperationCanceledException)
        {
            return Result<User>.Failure(
                IamError.OperationCancelled,
                localizer[$"{nameof(IamError)}.{IamError.OperationCancelled}"]);
        }
        catch (DbUpdateException)
        {
            return Result<User>.Failure(
                IamError.DatabaseError,
                localizer[$"{nameof(IamError)}.{IamError.DatabaseError}"]);
        }
        catch (Exception)
        {
            return Result<User>.Failure(
                IamError.InternalServerError,
                localizer[$"{nameof(IamError)}.{IamError.InternalServerError}"]);
        }
    }

    /// <summary>
    ///     Handles password changes requested from the authenticated user's Settings screen.
    /// </summary>
    /// <param name="command">Password change command containing the current and new plain passwords.</param>
    /// <param name="cancellationToken">Token used to cancel lookup and persistence work.</param>
    /// <returns>A successful result with the updated user, or an IAM error when validation fails.</returns>
    public async Task<Result<User>> Handle(ChangePasswordCommand command, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.FindByIdAsync(command.UserId, cancellationToken);
        if (user is null)
            return Result<User>.Failure(
                IamError.UserNotFound,
                localizer[$"{nameof(IamError)}.{IamError.UserNotFound}"]);

        if (!hashingService.VerifyPassword(command.CurrentPassword, user.PasswordHash))
            return Result<User>.Failure(
                IamError.InvalidCurrentPassword,
                localizer[$"{nameof(IamError)}.{IamError.InvalidCurrentPassword}"]);

        if (string.IsNullOrWhiteSpace(command.NewPassword) || command.NewPassword.Length < 6)
            return Result<User>.Failure(
                IamError.WeakPassword,
                localizer[$"{nameof(IamError)}.{IamError.WeakPassword}"]);

        try
        {
            user.ChangePasswordHash(hashingService.HashPassword(command.NewPassword));
            userRepository.Update(user);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<User>.Success(user);
        }
        catch (OperationCanceledException)
        {
            return Result<User>.Failure(
                IamError.OperationCancelled,
                localizer[$"{nameof(IamError)}.{IamError.OperationCancelled}"]);
        }
        catch (DbUpdateException)
        {
            return Result<User>.Failure(
                IamError.DatabaseError,
                localizer[$"{nameof(IamError)}.{IamError.DatabaseError}"]);
        }
        catch (Exception)
        {
            return Result<User>.Failure(
                IamError.InternalServerError,
                localizer[$"{nameof(IamError)}.{IamError.InternalServerError}"]);
        }
    }

    /// <summary>
    ///     Handles user sign-in and token issuance.
    /// </summary>
    /// <param name="command">Sign-in command containing the submitted email and password.</param>
    /// <param name="cancellationToken">Token used to cancel the user lookup.</param>
    /// <returns>
    ///     A successful result with the authenticated user and JWT token, or an invalid-credentials
    ///     result when the email does not exist, the account is inactive, or the password is wrong.
    /// </returns>
    public async Task<Result<(User user, string token)>> Handle(SignInCommand command, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.FindByEmailAsync(command.Email, cancellationToken);

        if (user is null || !user.IsActive || !hashingService.VerifyPassword(command.Password, user.PasswordHash))
            return Result<(User user, string token)>.Failure(
                IamError.InvalidCredentials,
                localizer[$"{nameof(IamError)}.{IamError.InvalidCredentials}"]);

        var token = tokenService.GenerateToken(user);
        return Result<(User user, string token)>.Success((user, token));
    }

    /// <summary>
    ///     Handles public user registration and initial token issuance.
    /// </summary>
    /// <param name="command">Sign-up command containing account fields and the plain password to hash.</param>
    /// <param name="cancellationToken">Token used to cancel persistence operations.</param>
    /// <returns>
    ///     A successful result with the created user and JWT token, or an IAM error for duplicated
    ///     email, cancellation, database failure or unexpected server failure.
    /// </returns>
    /// <remarks>
    ///     The sign-up workflow currently activates accounts immediately because Sprint 3 does not
    ///     include email confirmation or invitation acceptance.
    /// </remarks>
    public async Task<Result<(User user, string token)>> Handle(SignUpCommand command, CancellationToken cancellationToken = default)
    {
        if (!IsSupportedRole(command.Role) || string.Equals(command.Role, "owner", StringComparison.OrdinalIgnoreCase))
            return Result<(User user, string token)>.Failure(
                IamError.InvalidRole,
                localizer[$"{nameof(IamError)}.{IamError.InvalidRole}"]);

        if (string.IsNullOrWhiteSpace(command.Password) || command.Password.Length < 6)
            return Result<(User user, string token)>.Failure(
                IamError.WeakPassword,
                localizer[$"{nameof(IamError)}.{IamError.WeakPassword}"]);

        if (await userRepository.ExistsByEmailAsync(command.Email, cancellationToken))
            return Result<(User user, string token)>.Failure(
                IamError.EmailAlreadyTaken,
                localizer[$"{nameof(IamError)}.{IamError.EmailAlreadyTaken}"]);

        var passwordHash = hashingService.HashPassword(command.Password);
        var user = new User(command, passwordHash);

        try
        {
            await userRepository.AddAsync(user, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            var token = tokenService.GenerateToken(user);
            return Result<(User user, string token)>.Success((user, token));
        }
        catch (OperationCanceledException)
        {
            return Result<(User user, string token)>.Failure(
                IamError.OperationCancelled,
                localizer[$"{nameof(IamError)}.{IamError.OperationCancelled}"]);
        }
        catch (DbUpdateException)
        {
            return Result<(User user, string token)>.Failure(
                IamError.DatabaseError,
                localizer[$"{nameof(IamError)}.{IamError.DatabaseError}"]);
        }
        catch (Exception)
        {
            return Result<(User user, string token)>.Failure(
                IamError.InternalServerError,
                localizer[$"{nameof(IamError)}.{IamError.InternalServerError}"]);
        }
    }
}
