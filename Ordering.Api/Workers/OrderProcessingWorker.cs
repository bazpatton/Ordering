using Ordering.Api.Services.Interfaces;
using Polly;
using Polly.Retry;
using Polly.Timeout;

namespace Ordering.Api.Workers;

/// <summary>
/// The order processing worker.
/// </summary>
public class OrderProcessingWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IEventQueue _eventQueue;
    private readonly ILogger<OrderProcessingWorker> _logger;
    private readonly ResiliencePipeline _pipeline;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrderProcessingWorker"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider.</param>
    /// <param name="eventQueue">The event queue.</param>
    /// <param name="logger">The logger.</param>
    public OrderProcessingWorker(
        IServiceProvider serviceProvider,
        IEventQueue eventQueue,
        ILogger<OrderProcessingWorker> logger)
    {
        _serviceProvider = serviceProvider;
        _eventQueue = eventQueue;
        _logger = logger;

        // Configure retry policy for transient failures
        _pipeline = new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions
            {
                ShouldHandle = new PredicateBuilder().Handle<Exception>(),
                MaxRetryAttempts = 3,
                Delay = TimeSpan.FromSeconds(1),
                BackoffType = DelayBackoffType.Exponential,
                OnRetry = args =>
                {
                    _logger.LogWarning(
                        "Retry {RetryCount} after {Delay}s due to: {Exception}",
                        args.AttemptNumber, args.RetryDelay.TotalSeconds, args.Outcome.Exception?.Message);
                    return ValueTask.CompletedTask;
                }
            }).AddTimeout(new TimeoutStrategyOptions
            {
                Timeout = TimeSpan.FromSeconds(10)
            })
            .Build();
    }

    /// <summary>
    /// Executes the order processing worker.
    /// </summary>
    /// <param name="stoppingToken">The stopping token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Order Processing Worker started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var orderEvent = await _eventQueue.SubscribeAsync(stoppingToken);
                
                _logger.LogInformation("Received OrderCreated event for order {OrderId}", orderEvent.OrderId);

                // Process with retry policy
                await _pipeline.ExecuteAsync(async ct =>
                {
                    using var scope = _serviceProvider.CreateScope();
                    IOrderProcessor orderProcessor = scope.ServiceProvider.GetRequiredService<IOrderProcessor>();
                    await orderProcessor.ProcessOrderAsync(orderEvent.OrderId);
                }, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Order Processing Worker is stopping");
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process order after retries");
            }
        }

        _logger.LogInformation("Order Processing Worker stopped");
    }
}

