namespace Ordering.Api.Services.Interfaces;

/// <summary>
/// The order processor interface.
/// </summary>
public interface IOrderProcessor
{
    /// <summary>
    /// Processes an order.
    /// </summary>
    /// <param name="orderId">The ID of the order to process.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task ProcessOrderAsync(Guid orderId);
}



