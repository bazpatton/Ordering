using Ordering.Api.Events;

namespace Ordering.Api.Services.Interfaces;

/// <summary>
/// The event queue interface.
/// </summary>
public interface IEventQueue
{
    /// <summary>
    /// Publishes an order created event.
    /// </summary>
    /// <param name="orderEvent">The order created event.</param>
    /// <returns>A value task.</returns>
    ValueTask PublishAsync(OrderCreatedEvent orderEvent);
    /// <summary>
    /// Subscribes to an order created event.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The order created event.</returns>
    ValueTask<OrderCreatedEvent> SubscribeAsync(CancellationToken cancellationToken);
}



