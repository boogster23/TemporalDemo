using Temporalio.Workflows;

namespace TemporalDemo.Contracts;

public record OrderStatus(
        Guid OrderId,
        string CustomerNumber,
        decimal Amount,
        string CurrentState,
        string? PaymentTransactionId,
        string? FailureReason,
        DateTime CreatedAt,
        DateTime? CompletedAt);

[Workflow]
public interface IOrderWorkflow
{
    [WorkflowRun]
    Task<OrderStatus> RunAsync(SubmitOrder order);

    [WorkflowQuery]
    OrderStatus GetStatus();
}
