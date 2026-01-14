using Ordering.Api.DTOs;
using Ordering.Api.Models;

namespace Ordering.Api.Services
{
    /// <summary>
    /// The order response mapper.
    /// </summary>
    public class OrderResponseMapper
    {
        /// <summary>
        /// Maps an order to a response.
        /// </summary>
        /// <param name="order">The order to map.</param>
        /// <returns>The response.</returns>
        public static OrderResponse MapToResponse(Order order)
        {
            return new OrderResponse
            {
                OrderId = order.OrderId,
                CustomerId = order.CustomerId,
                Status = order.Status.ToString(),
                CreatedAt = order.CreatedAt,
                ProcessedAt = order.ProcessedAt,
                ErrorMessage = order.ErrorMessage,
                Items = order.Items.Select(i => new OrderItemResponse
                {
                    Sku = i.Sku,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()
            };
        }
    }
}