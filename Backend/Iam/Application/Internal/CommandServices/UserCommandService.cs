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

public class UserCommandService(
    IUserRepository userRepository,
    IHashingService hashingService,
    ITokenService tokenService,
    IUnitOfWork unitOfWork,
    IStringLocalizer<ErrorMessages> localizer)
    : IUserCommandService
{
    public async Task<Result<User>> Handle(CreateUserCommand command, CancellationToken cancellationToken = default)
    {
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

    public async Task<Result<User>> Handle(UpdateUserCommand command, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.FindByIdAsync(command.UserId, cancellationToken);
        if (user is null)
            return Result<User>.Failure(
                IamError.UserNotFound,
                localizer[$"{nameof(IamError)}.{IamError.UserNotFound}"]);

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
                command.IsActive);
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

    public async Task<Result<(User user, string token)>> Handle(SignUpCommand command, CancellationToken cancellationToken = default)
    {
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
