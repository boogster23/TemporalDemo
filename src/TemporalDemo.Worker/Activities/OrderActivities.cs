using TemporalDemo.Contracts;
using Temporalio.Activities;

namespace TemporalDemo.Worker.Activities;

public class OrderActivities(ILogger<OrderActivities> logger) : IOrderActivities
{
    [Activity]
    public async Task<PaymentResult> ProcessPaymentAsync(ProcessPayment payment)
    {
        logger.LogInformation(
            "--> [Payment Gateway] Charging ${Amount} to Customer: {Customer} for Order: {OrderId}...",
            payment.Amount,
            payment.CustomerNumber,
            payment.OrderId);

        await Task.Delay(300);

        if (payment.Amount > 1000)
        {
             logger.LogWarning(
                "--> [Payment Gateway] DECLINED Order: {OrderId}. Reason: Exceeds $1000 threshold.",
                payment.OrderId);

            return new PaymentResult(
                IsSuccess: false,
                TransactionId: null,
                FailureReason: "Declined: Exceeds $1000 threshold."
            );
        }

        var transactionId = $"TXN-{Guid.NewGuid().ToString()[..8].ToUpperInvariant()}";
        logger.LogInformation(
            "--> [Payment Gateway] APPROVED Order: {OrderId}. Transaction: {TxId}",
            payment.OrderId,
            transactionId);

        return new PaymentResult(
            IsSuccess: true,
            TransactionId: transactionId,
            FailureReason: null
        );
    }

    [Activity]
    public Task PublishOrderSubmittedAsync(OrderSubmitted orderSubmitted)
    {
        logger.LogInformation(
            "===> Order {OrderId} processed and OrderSubmitted published!",
            orderSubmitted.OrderId);
            
        return Task.CompletedTask;
    }

}
