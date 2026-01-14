namespace Ordering.Api.DTOs;

/// <summary>
/// The response to an order item.
/// </summary>
public class OrderItemResponse
{
    /// <summary>
    /// The SKU of the order item.
    /// </summary>
    public string Sku { get; set; } = string.Empty;
    /// <summary>
    /// The quantity of the order item.
    /// </summary>
    public int Quantity { get; set; }
    /// <summary>
    /// The price of the order item.
    /// </summary>
    public decimal Price { get; set; }
}




