namespace TemporalDemo.Contracts;

public record SubmitOrder
{
    public Guid OrderId { get; set; }
    public string CustomerNumber { get; init; } = default!;
    public decimal Amount { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}

public record OrderSubmitted
{
    public Guid OrderId { get; set; }
    public string CustomerNumber { get; set; } = default!;
    public decimal Amount { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
