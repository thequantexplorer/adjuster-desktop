using AdjusterDesktop.Core;
using Xunit;

public class ApprovalPolicyTests
{
    [Theory]
    [InlineData("Water damage")]
    [InlineData("Liability")]
    [InlineData("")]
    public void ClaimTypeWithoutItsOwnLimitUsesTheDefaultAdjusterLimit(string type)
    {
        Assert.Equal(ApprovalPolicy.AdjusterLimitCents, ApprovalPolicy.LimitCentsFor(type));
    }

    [Fact]
    public void ClaimTypeWithItsOwnLimitUsesIt()
    {
        Assert.Equal(500_000, ApprovalPolicy.LimitCentsFor("Theft"));
    }

    [Fact]
    public void DecidingAClaimOfAnUnlistedTypeDoesNotThrow()
    {
        var claim = new Claim { Id = "CLM-2004", Customer = "Sam Patel", Type = "Water damage", PayableCents = 1_230_000 };

        var decision = ApprovalPolicy.Decide(claim);

        Assert.Equal(ApprovalPolicy.Decide(claim, ApprovalPolicy.AdjusterLimitCents), decision);
    }

    [Fact]
    public void EverySampleClaimCanBeDecided()
    {
        var queue = ClaimQueue.Sample();

        foreach (var claim in queue.All.ToList())
        {
            Assert.NotEqual(ClaimStatus.InReview, queue.Approve(claim.Id));
        }
        Assert.Empty(queue.Pending);
    }
}
