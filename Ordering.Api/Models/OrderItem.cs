using System.ComponentModel.DataAnnotations;

namespace Ordering.Api.Models;

/// <summary>
/// The order item model.
/// </summary>  
public class OrderItem
{
    /// <summary>
    /// The ID of the order item.
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// The ID of the order.
    /// </summary>
    public Guid OrderId { get; set; }
    /// <summary>
    /// The SKU of the order item.
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Sku { get; set; } = string.Empty;
    
    /// <summary>
    /// The quantity of the order item.
    /// </summary>
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
    
    /// <summary>
    /// The price of the order item.
    /// </summary>
    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }
    
    /// <summary>
    /// The order the item belongs to.
    /// </summary>
    public Order? Order { get; set; }
}




