namespace TemporalDemo.Contracts;

public record ProcessPayment(
    Guid OrderId,
    string CustomerNumber,
    decimal Amount,
    DateTime CreatedAt);

public record PaymentResult(
    bool IsSuccess,
    string? TransactionId,
    string? FailureReason);

public record OrderPaymentCompleted(
    Guid OrderId,
    string PaymentTransactionId,
    DateTime ProcessedAt);

public record OrderPaymentFailed(
    Guid OrderId,
    string Reason);