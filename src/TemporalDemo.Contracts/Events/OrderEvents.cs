namespace TemporalDemo.Contracts;

public record SubmitOrder
{
    public Guid OrderId { get; }
    public string CustomerNumber { get; } = null!;
    public decimal Amount { get; }
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
}

public record OrderSubmitted
{
    public Guid OrderId { get; }
    public string CustomerNumber { get; } = null!;
    public decimal Amount { get; }
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
}
