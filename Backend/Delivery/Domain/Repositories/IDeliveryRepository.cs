using Buildline.Platform.Delivery.Domain.Model.Aggregates;
using Buildline.Platform.Shared.Domain.Repositories;

namespace Buildline.Platform.Delivery.Domain.Repositories;

/// <summary>Repository contract for delivery aggregate persistence.</summary>
public interface IDeliveryRepository : IBaseRepository<Domain.Model.Aggregates.Delivery>;
