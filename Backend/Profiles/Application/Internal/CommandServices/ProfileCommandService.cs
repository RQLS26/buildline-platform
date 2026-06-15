using Buildline.Platform.Profiles.Application.CommandServices;
using Buildline.Platform.Profiles.Domain.Model;
using Buildline.Platform.Profiles.Domain.Model.Aggregates;
using Buildline.Platform.Profiles.Domain.Model.Commands;
using Buildline.Platform.Profiles.Domain.Repositories;
using Buildline.Platform.Resources.Errors;
using Buildline.Platform.Shared.Application.Model;
using Buildline.Platform.Shared.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Buildline.Platform.Profiles.Application.Internal.CommandServices;

/// <summary>
///     Application command service for profile write use cases.
/// </summary>
public class ProfileCommandService(
    IProfileRepository profileRepository,
    IUnitOfWork unitOfWork,
    IStringLocalizer<ErrorMessages> localizer)
    : IProfileCommandService
{
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
