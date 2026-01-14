using System.ComponentModel.DataAnnotations;

namespace Ordering.Api.Models;

/// <summary>
/// The order model.
/// </summary>
public class Order
{
    /// <summary>
    /// The ID of the order.
    /// </summary>
    [Key]
    public Guid OrderId { get; set; }
    
    /// <summary>
    /// The ID of the customer.
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string CustomerId { get; set; } = string.Empty;
    
    /// <summary>
    /// The status of the order.
    /// </summary>
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    
    /// <summary>
    /// The date and time the order was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
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
    public List<OrderItem> Items { get; set; } = new();
}




