namespace Ordering.Api.Services.Interfaces
{
    public interface IPaymentProcesser
    {
        Task BillingSystemCallAsync(Guid orderId);
    }
}
