using Moq;
using Ordering.Api.Services.Interfaces;

namespace OrderingTestProject
{
    public class ProcessorTests
    {
        private readonly Mock<IPaymentProcesser> _mockPaymentProcessor;

        public ProcessorTests()
        {
            _mockPaymentProcessor = new Mock<IPaymentProcesser>();
        }

        [Fact]
        public async Task ProcessPayment_WithException()
        {
            var guid = Guid.NewGuid();

            // Arrange 
            _mockPaymentProcessor.Setup(s=> s.BillingSystemCallAsync(It.IsAny<Guid>())).Throws<InvalidOperationException>();

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _mockPaymentProcessor.Object.BillingSystemCallAsync(guid));

            _mockPaymentProcessor.Verify(s => s.BillingSystemCallAsync(guid), Times.Once);
        }

        [Fact]
        public async Task ProcessPayment_Successful()
        {
            var guid = Guid.NewGuid();

            // Arrange 
            _mockPaymentProcessor.Setup(s => s.BillingSystemCallAsync(It.IsAny<Guid>())).Returns(Task.CompletedTask);

            // Act
            await _mockPaymentProcessor.Object.BillingSystemCallAsync(guid);

            // Assert
            _mockPaymentProcessor.Verify(s => s.BillingSystemCallAsync(guid), Times.Once);
        }
    }
}
