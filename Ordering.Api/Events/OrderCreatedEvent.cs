namespace Ordering.Api.Events;

/// <summary>
/// The event that is published when an order is created.
/// </summary>
public class OrderCreatedEvent
{
    /// <summary>
    /// The ID of the order.
    /// </summary>
    public Guid OrderId { get; set; }
    /// <summary>
    /// The ID of the customer.
    /// </summary>
    public string CustomerId { get; set; } = string.Empty;
    /// <summary>
    /// The date and time the order was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}




