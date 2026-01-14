using Ordering.Api.Services.Interfaces;

namespace Ordering.Api.Services
{
    /// <summary>
    /// The payment processer.
    /// </summary>
    public class PaymentProcesser : IPaymentProcesser
    {
        private readonly Random _random = new();
        private readonly ILogger<PaymentProcesser> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentProcesser"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public PaymentProcesser(ILogger<PaymentProcesser> logger)
        {
            _logger = logger;
        }

        /// Calls the billing system.
        /// </summary>
        /// <param name="orderId">The ID of the order.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task BillingSystemCallAsync(Guid orderId)
        {
            // Simulate network delay
            await Task.Delay(_random.Next(100, 500));

            // Simulate transient failures (20% failure rate)
            if (_random.Next(100) < 20)
            {
                throw new InvalidOperationException("Simulated transient billing system failure");
            }

            _logger.LogInformation("Billing system processed order {OrderId}", orderId);
        }
    }
}
