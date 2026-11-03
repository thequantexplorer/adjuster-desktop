namespace AdjusterDesktop.Core;

public enum ClaimStatus { InReview, Approved, SentToSupervisor, Denied }

public sealed class Claim
{
    public required string Id { get; init; }
    public required string Customer { get; init; }
    public required string Type { get; init; }
    /// <summary>Amount Northwind pays after the deductible, in cents.</summary>
    public required long PayableCents { get; init; }
    public ClaimStatus Status { get; set; } = ClaimStatus.InReview;

    public string PayableDisplay => $"${PayableCents / 100:N0}.{PayableCents % 100:00}";
}
