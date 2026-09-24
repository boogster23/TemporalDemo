using TemporalDemo.Contracts;
using Temporalio.Workflows;

namespace TemporalDemo.Worker.Workflows;

public class OrderWorkflow : IOrderWorkflow
{
    private OrderStatus? _status;

    [WorkflowRun]
    public async Task<OrderStatus> RunAsync(SubmitOrder order)
    {
        _status = new OrderStatus(
            OrderId: order.OrderId,
            CustomerNumber: order.CustomerNumber,
            Amount: order.Amount,
            CurrentState: "Submitted",
            PaymentTransactionId: null,
            FailureReason: null,
            CreatedAt: order.CreatedAt,
            CompletedAt: null);

        await Workflow.ExecuteActivityAsync(
            (IOrderActivities act) => act.PublishOrderSubmittedAsync(
                new OrderSubmitted
                {
                    OrderId = order.OrderId,
                    CustomerNumber = order.CustomerNumber,
                    Amount = order.Amount,
                    CreatedAt = order.CreatedAt
                }),
                new ActivityOptions { StartToCloseTimeout = TimeSpan.FromMinutes(1) });

        var paymentResult = await Workflow.ExecuteActivityAsync(
            (IOrderActivities act) => act.ProcessPaymentAsync(
                new ProcessPayment
                (
                    OrderId: order.OrderId,
                    CustomerNumber: order.CustomerNumber,
                    Amount: order.Amount,
                    CreatedAt: Workflow.UtcNow
                )),
                new ActivityOptions { StartToCloseTimeout = TimeSpan.FromMinutes(1) });
        
        if (paymentResult.IsSuccess)
        {
            _status = _status with
            {
                CurrentState = "Cancelled",
                FailureReason = paymentResult.FailureReason
            };
        }
        else
        {
            _status = _status with
            {
                CurrentState = "PaymentFailed",
                FailureReason = paymentResult.FailureReason,
                CompletedAt = DateTime.UtcNow
            };
        }

        return _status ?? throw new InvalidOperationException("Order status is not initialized.");
    }

    [WorkflowQuery]
    public OrderStatus GetStatus() => _status ?? throw new InvalidOperationException("Order status is not initialized.");
}
