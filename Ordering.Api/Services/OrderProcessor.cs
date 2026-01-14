using Ordering.Api.Data;
using Ordering.Api.Models;
using Ordering.Api.Services.Interfaces;

namespace Ordering.Api.Services;

/// <summary>
/// The order processor.
/// </summary>
public class OrderProcessor : IOrderProcessor
{
    private readonly ILogger<OrderProcessor> _logger;
    private readonly IOrderService _orderService;
    private readonly IPaymentProcesser _paymentProcesser;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrderProcessor"/> class.
    /// </summary>
    /// <param name="orderService">Order Service</param>
    /// <param name="logger">The logger.</param>
    /// <param name="paymentProcesser">Payment Processor</param>
    public OrderProcessor(IOrderService orderService, ILogger<OrderProcessor> logger, IPaymentProcesser paymentProcesser)
    {
        _orderService = orderService;
        _logger = logger;
        _paymentProcesser = paymentProcesser;
    }


    /// <summary>
    /// Processes an order.
    /// </summary>
    /// <param name="orderId">The ID of the order to process.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task ProcessOrderAsync(Guid orderId)
    {
        var order = await _orderService.GetOrderAsync(orderId);
        if (order == null)
        {
            _logger.LogWarning("Order {OrderId} not found", orderId);
            return;
        }

        try
        {
            _logger.LogInformation("Processing order {OrderId}", orderId);

            await _paymentProcesser.BillingSystemCallAsync(orderId);

            order.Status = OrderStatus.Processed;
            order.ProcessedAt = DateTime.UtcNow;
            order.ErrorMessage = null;

            await _orderService.UpdateOrder(order);

            _logger.LogInformation("Order {OrderId} processed successfully", orderId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to process order {OrderId}", orderId);
            
            order.Status = OrderStatus.Failed;
            order.ErrorMessage = ex.Message;

            await _orderService.UpdateOrder(order);

            throw;
        }
    }
}




