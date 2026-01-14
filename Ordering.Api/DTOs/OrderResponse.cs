using System.ComponentModel.DataAnnotations;

namespace Ordering.Api.DTOs;

/// <summary>
/// The response to an order.
/// </summary>
public class OrderResponse
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
    /// The status of the order.
    /// </summary>
    public string Status { get; set; } = string.Empty;
    /// <summary>
    /// The date and time the order was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }
    /// <summary>
    /// The date and time the order was processed.
    /// </summary>
    public DateTime? ProcessedAt { get; set; }
    /// <summary>
    /// The error message of the order.
    /// </summary>
    public string? ErrorMessage { get; set; }
    /// <summary>
    /// The items in the order.
    /// </summary>
    [Required]
    [MinLength(1)]
    public List<OrderItemResponse> Items { get; set; } = new();
}




