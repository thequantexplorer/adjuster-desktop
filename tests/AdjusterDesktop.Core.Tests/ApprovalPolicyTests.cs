using AdjusterDesktop.Core;
using Xunit;

public class ApprovalPolicyTests
{
    private static Claim ClaimFor(long payableCents) => new()
    {
        Id = "CLM-TEST",
        Customer = "Test Customer",
        Type = "Auto",
        PayableCents = payableCents,
    };

    [Theory]
    [InlineData(0L)]
    [InlineData(270_000L)]   // $2,700.00
    [InlineData(999_999L)]   // $9,999.99
    public void BelowTheLimitIsApproved(long payableCents)
    {
        Assert.Equal(ClaimStatus.Approved, ApprovalPolicy.Decide(ClaimFor(payableCents)));
    }

    [Fact]
    public void ExactlyAtTheLimitIsApproved()
    {
        Assert.Equal(ClaimStatus.Approved, ApprovalPolicy.Decide(ClaimFor(ApprovalPolicy.AdjusterLimitCents)));
    }

    [Theory]
    [InlineData(1_000_001L)]   // $10,000.01
    [InlineData(1_230_000L)]   // $12,300.00
    [InlineData(2_000_000L)]   // $20,000.00 (reported claim)
    public void AboveTheLimitIsSentToSupervisor(long payableCents)
    {
        Assert.Equal(ClaimStatus.SentToSupervisor, ApprovalPolicy.Decide(ClaimFor(payableCents)));
    }

    [Fact]
    public void AdjusterLimitIsTenThousandDollars()
    {
        Assert.Equal(10_000L * 100, ApprovalPolicy.AdjusterLimitCents);
    }

    [Fact]
    public void CustomLimitIsComparedInCents()
    {
        Assert.Equal(ClaimStatus.Approved, ApprovalPolicy.Decide(ClaimFor(50_000), limitCents: 50_000));
        Assert.Equal(ClaimStatus.SentToSupervisor, ApprovalPolicy.Decide(ClaimFor(50_001), limitCents: 50_000));
    }
}
