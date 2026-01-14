using System.ComponentModel.DataAnnotations;

namespace Ordering.Api.DTOs;

/// <summary>
/// The request to create an order.
/// </summary>
public class CreateOrderRequest
{
    /// <summary>
    /// The ID of the order.
    /// </summary>
    public Guid? OrderId { get; set; }
    /// <summary>
    /// The ID of the customer.
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string CustomerId { get; set; } = string.Empty;
    
    /// <summary>
    /// The items in the order.
    /// </summary>
    [Required]
    [MinLength(1)]
    public List<OrderItemRequest> Items { get; set; } = new();
}




