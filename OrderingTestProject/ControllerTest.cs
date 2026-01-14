using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Ordering.Api.Controllers;
using Ordering.Api.DTOs;
using Ordering.Api.Services.Interfaces;

namespace OrderingTestProject
{
    public class OrdersControllerTests
    {
        private readonly Mock<IOrderService> _mockOrderService;
        private readonly Mock<ILogger<OrdersController>> _mockLogger;
        private readonly OrdersController _controller;

        public OrdersControllerTests()
        {
            _mockOrderService = new Mock<IOrderService>();
            _mockLogger = new Mock<ILogger<OrdersController>>();
            _controller = new OrdersController(_mockOrderService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task CreateOrder_WithValidRequest_ReturnsCreatedResult()
        {
            // Arrange
            var request = new CreateOrderRequest
            {
                CustomerId = "customer123",
                Items = new List<OrderItemRequest>
                {
                    new OrderItemRequest { Sku = "SKU001", Quantity = 2, Price = 19.99m }
                }
            };

            var expectedResponse = new OrderResponse
            {
                OrderId = Guid.NewGuid(),
                CustomerId = "customer123",
                Status = "Pending",
                CreatedAt = DateTime.UtcNow,
                Items = new List<OrderItemResponse>
                {
                    new OrderItemResponse { Sku = "SKU001", Quantity = 2, Price = 19.99m }
                }
            };

            _mockOrderService
                .Setup(s => s.CreateOrderAsync(It.IsAny<CreateOrderRequest>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.CreateOrder(request);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(nameof(OrdersController.GetOrder), createdResult.ActionName);
            Assert.NotNull(createdResult.RouteValues);
            Assert.True(createdResult.RouteValues.ContainsKey("id"));
            Assert.Equal(expectedResponse.OrderId, createdResult.RouteValues["id"]);
            
            var returnedOrder = Assert.IsType<OrderResponse>(createdResult.Value);
            Assert.Equal(expectedResponse.OrderId, returnedOrder.OrderId);
            Assert.Equal(expectedResponse.CustomerId, returnedOrder.CustomerId);
            Assert.Equal(expectedResponse.Status, returnedOrder.Status);
            
            _mockOrderService.Verify(s => s.CreateOrderAsync(request), Times.Once);
        }

        [Fact]
        public async Task CreateOrder_WithInvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            var request = new CreateOrderRequest
            {
                CustomerId = "customer123",
                Items = new List<OrderItemRequest>()
            };

            _controller.ModelState.AddModelError("Items", "Items field is required");

            // Act
            var result = await _controller.CreateOrder(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
            
            _mockOrderService.Verify(s => s.CreateOrderAsync(It.IsAny<CreateOrderRequest>()), Times.Never);
        }

        [Fact]
        public async Task CreateOrder_WhenServiceThrowsException_ReturnsInternalServerError()
        {
            // Arrange
            var request = new CreateOrderRequest
            {
                CustomerId = "customer123",
                Items = new List<OrderItemRequest>
                {
                    new OrderItemRequest { Sku = "SKU001", Quantity = 1, Price = 10.00m }
                }
            };

            _mockOrderService
                .Setup(s => s.CreateOrderAsync(It.IsAny<CreateOrderRequest>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.CreateOrder(request);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, objectResult.StatusCode);
            
            var errorResponse = objectResult.Value as dynamic;
            Assert.NotNull(errorResponse);
            
            _mockOrderService.Verify(s => s.CreateOrderAsync(request), Times.Once);
        }

        [Fact]
        public async Task GetOrder_WithExistingId_ReturnsOkResult()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var expectedOrder = new OrderResponse
            {
                OrderId = orderId,
                CustomerId = "customer123",
                Status = "Completed",
                CreatedAt = DateTime.UtcNow,
                Items = new List<OrderItemResponse>
                {
                    new OrderItemResponse { Sku = "SKU001", Quantity = 1, Price = 25.50m }
                }
            };

            _mockOrderService
                .Setup(s => s.GetOrderResponceAsync(orderId))
                .ReturnsAsync(expectedOrder);

            // Act
            var result = await _controller.GetOrder(orderId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedOrder = Assert.IsType<OrderResponse>(okResult.Value);
            Assert.Equal(expectedOrder.OrderId, returnedOrder.OrderId);
            Assert.Equal(expectedOrder.CustomerId, returnedOrder.CustomerId);
            Assert.Equal(expectedOrder.Status, returnedOrder.Status);
            
            _mockOrderService.Verify(s => s.GetOrderResponceAsync(orderId), Times.Once);
        }

        [Fact]
        public async Task GetOrder_WithNonExistingId_ReturnsNotFound()
        {
            // Arrange
            var orderId = Guid.NewGuid();

            _mockOrderService
                .Setup(s => s.GetOrderResponceAsync(orderId))
                .ReturnsAsync((OrderResponse?)null);

            // Act
            var result = await _controller.GetOrder(orderId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var errorResponse = notFoundResult.Value as dynamic;
            Assert.NotNull(errorResponse);
            
            _mockOrderService.Verify(s => s.GetOrderResponceAsync(orderId), Times.Once);
        }

        [Fact]
        public async Task GetOrder_WhenServiceThrowsException_ReturnsInternalServerError()
        {
            // Arrange
            var orderId = Guid.NewGuid();

            _mockOrderService
                .Setup(s => s.GetOrderResponceAsync(orderId))
                .ThrowsAsync(new Exception("Database connection failed"));

            // Act
            var result = await _controller.GetOrder(orderId);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, objectResult.StatusCode);
            
            var errorResponse = objectResult.Value as dynamic;
            Assert.NotNull(errorResponse);
            
            _mockOrderService.Verify(s => s.GetOrderResponceAsync(orderId), Times.Once);
        }

        [Fact]
        public async Task GetAllOrders_WithExistingOrders_ReturnsOkResultWithOrders()
        {
            // Arrange
            var expectedOrders = new List<OrderResponse>
            {
                new OrderResponse
                {
                    OrderId = Guid.NewGuid(),
                    CustomerId = "customer1",
                    Status = "Completed",
                    CreatedAt = DateTime.UtcNow,
                    Items = new List<OrderItemResponse>
                    {
                        new OrderItemResponse { Sku = "SKU001", Quantity = 1, Price = 10.00m }
                    }
                },
                new OrderResponse
                {
                    OrderId = Guid.NewGuid(),
                    CustomerId = "customer2",
                    Status = "Pending",
                    CreatedAt = DateTime.UtcNow,
                    Items = new List<OrderItemResponse>
                    {
                        new OrderItemResponse { Sku = "SKU002", Quantity = 2, Price = 20.00m }
                    }
                }
            };

            _mockOrderService
                .Setup(s => s.GetAllOrders())
                .ReturnsAsync(expectedOrders);

            // Act
            var result = await _controller.GetAllOrders();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedOrders = Assert.IsType<List<OrderResponse>>(okResult.Value);
            Assert.Equal(2, returnedOrders.Count);
            Assert.Equal(expectedOrders[0].OrderId, returnedOrders[0].OrderId);
            Assert.Equal(expectedOrders[1].OrderId, returnedOrders[1].OrderId);
            
            _mockOrderService.Verify(s => s.GetAllOrders(), Times.Once);
        }

        [Fact]
        public async Task GetAllOrders_WithNoOrders_ReturnsOkResultWithEmptyList()
        {
            // Arrange
            var expectedOrders = new List<OrderResponse>();

            _mockOrderService
                .Setup(s => s.GetAllOrders())
                .ReturnsAsync(expectedOrders);

            // Act
            var result = await _controller.GetAllOrders();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedOrders = Assert.IsType<List<OrderResponse>>(okResult.Value);
            Assert.Empty(returnedOrders);
            
            _mockOrderService.Verify(s => s.GetAllOrders(), Times.Once);
        }

        [Fact]
        public async Task GetAllOrders_WhenServiceThrowsException_ReturnsInternalServerError()
        {
            // Arrange
            _mockOrderService
                .Setup(s => s.GetAllOrders())
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.GetAllOrders();

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, objectResult.StatusCode);
            
            var errorResponse = objectResult.Value as dynamic;
            Assert.NotNull(errorResponse);
            
            _mockOrderService.Verify(s => s.GetAllOrders(), Times.Once);
        }
    }
}