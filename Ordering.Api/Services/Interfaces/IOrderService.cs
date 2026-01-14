using Ordering.Api.DTOs;
using Ordering.Api.Models;

namespace Ordering.Api.Services.Interfaces;

/// <summary>
/// The order service interface.
/// </summary>
public interface IOrderService
{
    /// <summary>
    /// Creates an order.
    /// </summary>
    /// <param name="request">The request containing the order details.</param>
    /// <returns>The created order.</returns>
    Task<OrderResponse> CreateOrderAsync(CreateOrderRequest request);
    /// <summary>
    /// Gets an order by its ID.
    /// </summary>
    /// <param name="orderId">The ID of the order to get.</param>
    /// <returns>The order.</returns>
    Task<OrderResponse?> GetOrderResponceAsync(Guid orderId);

    /// <summary>
    /// Gets an order by its ID.
    /// </summary>
    /// <param name="orderId">The ID of the order to get.</param>
    /// <returns>The order.</returns>
    Task<Order?> GetOrderAsync(Guid orderId);

    /// <summary>
    /// Gets all orders.
    /// </summary>
    /// <returns>The list of orders.</returns>
    Task<List<OrderResponse>> GetAllOrders();

    /// <summary>
    /// Update Order
    /// </summary>
    /// <param name="order"></param>
    /// <returns>Completed Task</returns>
    Task UpdateOrder(Order order);
}



