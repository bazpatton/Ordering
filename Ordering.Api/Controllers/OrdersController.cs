using Microsoft.AspNetCore.Mvc;
using Ordering.Api.DTOs;
using Ordering.Api.Services.Interfaces;

namespace Ordering.Api.Controllers;

/// <summary>
/// The orders controller for the ordering system.
/// </summary>
[ApiController]
[Route("orders")]
public class OrdersController : ControllerBase
{
    /// <summary>
    /// The order service for the ordering system.
    /// </summary>
    private readonly IOrderService _orderService;
    /// <summary>
    /// The logger for the orders controller.
    /// </summary>
    private readonly ILogger<OrdersController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrdersController"/> class.
    /// </summary>
    /// <param name="orderService">The order service for the ordering system.</param>
    /// <param name="logger">The logger for the orders controller.</param>
    public OrdersController(IOrderService orderService, ILogger<OrdersController> logger)
    {
        _orderService = orderService;
        _logger = logger;
    }

    /// <summary>
    /// Creates a new order.
    /// </summary>
    /// <param name="request">The request containing the order details.</param>
    /// <returns>The created order.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<OrderResponse>> CreateOrder([FromBody] CreateOrderRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var order = await _orderService.CreateOrderAsync(request);
            return CreatedAtAction(nameof(GetOrder), new { id = order.OrderId }, order);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating order");
            return StatusCode(500, new { error = "An error occurred while creating the order" });
        }
    }

    /// <summary>
    /// Gets an order by its ID.
    /// </summary>
    /// <param name="id">The ID of the order to get.</param>
    /// <returns>The order.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OrderResponse>> GetOrder(Guid id)
    {
        try
        {
            var order = await _orderService.GetOrderResponceAsync(id);
            if (order == null)
            {
                return NotFound(new { error = $"Order {id} not found" });
            }

            return Ok(order);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving order {OrderId}", id);
            return StatusCode(500, new { error = "An error occurred while retrieving the order" });
        }
    }

    /// <summary>
    /// Gets all orders.
    /// </summary>
    /// <returns>The list of orders.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<OrderResponse>>> GetAllOrders()
    {
        try
        {
            var orders = await _orderService.GetAllOrders();
            return Ok(orders);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving orders");
            return StatusCode(500, new { error = "An error occurred while retrieving the orders" });
        }
    }
}




