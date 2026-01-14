using Microsoft.EntityFrameworkCore;
using Ordering.Api.Data;
using Ordering.Api.DTOs;
using Ordering.Api.Events;
using Ordering.Api.Models;
using Ordering.Api.Services.Interfaces;

namespace Ordering.Api.Services;

/// <summary>
/// The order service.
/// </summary>
public class OrderService : IOrderService
{
    private readonly OrderingDbContext _context;
    private readonly IEventQueue _eventQueue;
    private readonly ILogger<OrderService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrderService"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    /// <param name="eventQueue">The event queue.</param>
    /// <param name="logger">The logger.</param>
    public OrderService(
        OrderingDbContext context,
        IEventQueue eventQueue,
        ILogger<OrderService> logger)
    {
        _context = context;
        _eventQueue = eventQueue;
        _logger = logger;
    }

    /// <summary>
    /// Creates an order.
    /// </summary>
    /// <param name="request">The request containing the order details.</param>
    /// <returns>The created order.</returns>
    public async Task<OrderResponse> CreateOrderAsync(CreateOrderRequest request)
    {
        try
        {
            var order = new Order
            {
                OrderId = request.OrderId ?? Guid.NewGuid(),
                CustomerId = request.CustomerId,
                Status = OrderStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                Items = request.Items.Select(i => new OrderItem
                {
                    Sku = i.Sku,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Order {OrderId} created for customer {CustomerId}",
                order.OrderId, order.CustomerId);

            // Publish event
            await _eventQueue.PublishAsync(new OrderCreatedEvent
            {
                OrderId = order.OrderId,
                CustomerId = order.CustomerId,
                CreatedAt = order.CreatedAt
            });

            return OrderResponseMapper.MapToResponse(order);
        }
        catch (Exception)
        {
            throw;
        }
    }

    /// <summary>
    /// Gets all orders.
    /// </summary>
    /// <returns>The list of orders.</returns>
    public async Task<List<OrderResponse>> GetAllOrders()
    {
        try
        {
            return await _context.Orders
            .Include(o => o.Items)
            .Select(o => OrderResponseMapper.MapToResponse(o))
            .ToListAsync();
        }
        catch (Exception)
        {

            throw;
        }
    }

    /// <summary>
    /// Gets an order by its ID.
    /// </summary>
    /// <param name="orderId">The ID of the order to get.</param>
    /// <returns>The order.</returns>
    public async Task<Order?> GetOrderAsync(Guid orderId)
    {
        try
        {
            var order = await _context.Orders
        .Include(o => o.Items)
        .FirstOrDefaultAsync(o => o.OrderId == orderId);

            return order != null ? order : null;
        }
        catch (Exception)
        {

            throw;
        }
    }

    /// <summary>
    /// Gets an order by its ID.
    /// </summary>
    /// <param name="orderId">The ID of the order to get.</param>
    /// <returns>The order.</returns>
    public async Task<OrderResponse?> GetOrderResponceAsync(Guid orderId)
    {
        try
        {
            var order = await _context.Orders
        .Include(o => o.Items)
        .FirstOrDefaultAsync(o => o.OrderId == orderId);

            return order != null ? OrderResponseMapper.MapToResponse(order) : null;
        }
        catch (Exception)
        {

            throw;
        }
    }

    /// <summary>
    /// Updates an existing order.
    /// </summary>
    /// <param name="order">The order to update.</param>
    public async Task UpdateOrder(Order order)
    {
        try
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            
            _logger.LogInformation("Order {OrderId} updated with status {Status}",
                order.OrderId, order.Status);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating order {OrderId}", order.OrderId);
            throw;
        }
    }
}




