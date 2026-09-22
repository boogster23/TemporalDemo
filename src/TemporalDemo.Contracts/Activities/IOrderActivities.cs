using Temporalio.Activities;

namespace TemporalDemo.Contracts;

public interface IOrderActivities
{
    [Activity]
    Task<OrderStatus> ProcessPaymentAsync(ProcessPayment payment);

    [Activity]
    Task PublishOrderSubmittedAsync(OrderSubmitted orderSubmitted);
}
