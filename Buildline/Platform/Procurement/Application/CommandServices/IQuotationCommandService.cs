using Buildline.Platform.Procurement.Domain.Model.Aggregates;
using Buildline.Platform.Procurement.Domain.Model.Commands;
using Buildline.Platform.Shared.Application.Model;

namespace Buildline.Platform.Procurement.Application.CommandServices;

/// <summary>
///     Defines application write use cases for quotation aggregates.
/// </summary>
public interface IQuotationCommandService
{
    /// <summary>
    ///     Handles creation of a new quotation aggregate.
    /// </summary>
    /// <param name="command">Command containing the values accepted by the REST contract.</param>
    /// <param name="cancellationToken">Token used to cancel repository and unit-of-work operations.</param>
    /// <returns>A successful result with the created aggregate, or a typed bounded-context error.</returns>
    Task<Result<Quotation>> Handle(CreateQuotationCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Handles partial update of an existing quotation aggregate.
    /// </summary>
    /// <param name="command">Command containing the aggregate id and replacement values.</param>
    /// <param name="cancellationToken">Token used to cancel lookup, mutation and persistence.</param>
    /// <returns>A successful result with the updated aggregate, or a typed bounded-context error.</returns>
    Task<Result<Quotation>> Handle(UpdateQuotationCommand command, CancellationToken cancellationToken = default);
}
