using Buildline.Platform.Profiles.Application.CommandServices;
using Buildline.Platform.Profiles.Domain.Model;
using Buildline.Platform.Profiles.Domain.Model.Aggregates;
using Buildline.Platform.Profiles.Domain.Model.Commands;
using Buildline.Platform.Profiles.Domain.Repositories;
using Buildline.Platform.Profiles.Resources;
using Buildline.Platform.Shared.Application.Model;
using Buildline.Platform.Shared.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Buildline.Platform.Profiles.Application.Internal.CommandServices;

/// <summary>
///     Application command service for profile write use cases.
/// </summary>
/// <param name="profileRepository">Repository used to retrieve and persist company profiles.</param>
/// <param name="unitOfWork">Unit of work used to commit profile changes transactionally.</param>
/// <param name="localizer">Localizer used to resolve profile error messages.</param>
public class ProfileCommandService(
    IProfileRepository profileRepository,
    IUnitOfWork unitOfWork,
    IStringLocalizer<ProfilesMessages> localizer)
    : IProfileCommandService
{
    /// <summary>
    ///     Handles company profile update.
    /// </summary>
    /// <param name="command">Update command containing profile id and replacement company data.</param>
    /// <param name="cancellationToken">Token used to cancel lookup, mutation and persistence.</param>
    /// <returns>
    ///     A successful result with the updated profile, or a profile-domain error when the profile
    ///     does not exist, persistence fails, or the operation is cancelled.
    /// </returns>
    public async Task<Result<Profile>> Handle(UpdateProfileCommand command, CancellationToken cancellationToken = default)
    {
        var profile = await profileRepository.FindByIdAsync(command.ProfileId, cancellationToken);
        if (profile is null)
            return Result<Profile>.Failure(
                ProfilesError.ProfileNotFound,
                localizer[$"{nameof(ProfilesError)}.{ProfilesError.ProfileNotFound}"]);

        try
        {
            profile.UpdateCompanyInformation(
                command.CompanyName,
                command.Ruc,
                command.Address,
                command.Phone,
                command.Email);
            profileRepository.Update(profile);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Profile>.Success(profile);
        }
        catch (OperationCanceledException)
        {
            return Result<Profile>.Failure(
                ProfilesError.OperationCancelled,
                localizer[$"{nameof(ProfilesError)}.{ProfilesError.OperationCancelled}"]);
        }
        catch (DbUpdateException)
        {
            return Result<Profile>.Failure(
                ProfilesError.DatabaseError,
                localizer[$"{nameof(ProfilesError)}.{ProfilesError.DatabaseError}"]);
        }
        catch (Exception)
        {
            return Result<Profile>.Failure(
                ProfilesError.InternalServerError,
                localizer[$"{nameof(ProfilesError)}.{ProfilesError.InternalServerError}"]);
        }
    }
}
