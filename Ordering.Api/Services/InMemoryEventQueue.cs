using System.Threading.Channels;
using Ordering.Api.Events;
using Ordering.Api.Services.Interfaces;

namespace Ordering.Api.Services;

/// <summary>
/// The in-memory event queue.
/// </summary>
public class InMemoryEventQueue : IEventQueue
{
    private readonly Channel<OrderCreatedEvent> _channel;

    /// <summary>
    /// Initializes a new instance of the <see cref="InMemoryEventQueue"/> class.
    /// </summary>
    public InMemoryEventQueue()
    {
        var options = new BoundedChannelOptions(100)
        {
            FullMode = BoundedChannelFullMode.Wait
        };
        _channel = Channel.CreateBounded<OrderCreatedEvent>(options);
    }
    /// <summary>
    /// Publishes an order created event.
    /// </summary>
    /// <param name="orderEvent">The order created event.</param>
    /// <returns>A value task.</returns>
    public async ValueTask PublishAsync(OrderCreatedEvent orderEvent)
    {
        await _channel.Writer.WriteAsync(orderEvent);
    }

    /// <summary>
    /// Subscribes to an order created event.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The order created event.</returns>
    public async ValueTask<OrderCreatedEvent> SubscribeAsync(CancellationToken cancellationToken)
    {
        return await _channel.Reader.ReadAsync(cancellationToken);
    }
}




