using Temporalio.Activities;

namespace TemporalDemo.Contracts;

public interface IOrderActivities
{
    [Activity]
     Task<PaymentResult> ProcessPaymentAsync(ProcessPayment payment);

    [Activity]
    Task PublishOrderSubmittedAsync(OrderSubmitted orderSubmitted);
}
