using QuantityMeasurementApp.Api.Contracts;

namespace QuantityMeasurementApp.Api.Messaging;

public interface IOperationEventPublisher
{
    Task PublishAsync(OperationEventDto message, CancellationToken cancellationToken = default);
}
