using System.ComponentModel.DataAnnotations;

namespace Ordering.Api.DTOs;

/// <summary>
/// The request to create an order item.
/// </summary>
public class OrderItemRequest
{
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
}




