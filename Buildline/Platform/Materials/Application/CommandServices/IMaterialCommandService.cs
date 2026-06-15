using Buildline.Platform.Materials.Domain.Model.Aggregates;
using Buildline.Platform.Materials.Domain.Model.Commands;
using Buildline.Platform.Shared.Application.Model;

namespace Buildline.Platform.Materials.Application.CommandServices;

/// <summary>
///     Defines write operations exposed by the Materials application layer.
/// </summary>
/// <remarks>
///     Command services coordinate validation, aggregate mutation, repository operations and
///     unit-of-work commits for TS-05, TS-07 and TS-08.
/// </remarks>
public interface IMaterialCommandService
{
    /// <summary>
    ///     Handles material creation.
    /// </summary>
    /// <param name="command">Command containing catalog and stock data for a new material.</param>
    /// <param name="cancellationToken">Token used to cancel persistence operations.</param>
    /// <returns>A result containing the created material or a material-domain error.</returns>
    Task<Result<Material>> Handle(CreateMaterialCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handles material update.
    /// </summary>
    /// <param name="command">Command containing the target id and replacement material data.</param>
    /// <param name="cancellationToken">Token used to cancel lookup and persistence operations.</param>
    /// <returns>A result containing the updated material or a material-domain error.</returns>
    Task<Result<Material>> Handle(UpdateMaterialCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handles material deletion.
    /// </summary>
    /// <param name="command">Command containing the material id that must be removed.</param>
    /// <param name="cancellationToken">Token used to cancel lookup and persistence operations.</param>
    /// <returns>A result that indicates success or a material-domain error.</returns>
    Task<Result> Handle(DeleteMaterialCommand command, CancellationToken cancellationToken = default);
}
