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
    public Guid OrderId { get; set; }
    public string CustomerNumber { get; set; } = null!;
    public decimal Amount { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
