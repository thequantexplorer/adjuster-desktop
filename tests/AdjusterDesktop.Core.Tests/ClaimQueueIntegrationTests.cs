using AdjusterDesktop.Core;
using Xunit;

[Trait("Category", "Integration")]
public class ClaimQueueIntegrationTests
{
    [Fact]
    public void ApprovingEverySampleClaimRoutesEachByTheAdjusterLimit()
    {
        var queue = ClaimQueue.Sample();
        Assert.Equal(4, queue.Pending.Count);

        var results = queue.All.Select(c => c.Id).ToList()
            .ToDictionary(id => id, queue.Approve);

        Assert.Equal(ClaimStatus.Approved, results["CLM-1002"]);          // $2,700.00
        Assert.Equal(ClaimStatus.Approved, results["CLM-1007"]);          // $8,450.50
        Assert.Equal(ClaimStatus.SentToSupervisor, results["CLM-2001"]);  // $20,000.00
        Assert.Equal(ClaimStatus.SentToSupervisor, results["CLM-2004"]);  // $12,300.00

        Assert.Empty(queue.Pending);
        Assert.All(queue.All, c => Assert.Equal(results[c.Id], c.Status));
    }

    [Fact]
    public void TwentyThousandDollarClaimIsSentToSupervisorNotApproved()
    {
        var queue = ClaimQueue.Sample();

        var status = queue.Approve("CLM-2001");

        Assert.Equal(ClaimStatus.SentToSupervisor, status);
        Assert.Equal(ClaimStatus.SentToSupervisor, queue.All.Single(c => c.Id == "CLM-2001").Status);
        Assert.DoesNotContain(queue.Pending, c => c.Id == "CLM-2001");
        Assert.Equal(3, queue.Pending.Count);
    }

    [Fact]
    public void ClaimsAtTheBoundaryAreRoutedCorrectly()
    {
        var queue = new ClaimQueue(new[]
        {
            new Claim { Id = "AT", Customer = "A", Type = "Auto", PayableCents = ApprovalPolicy.AdjusterLimitCents },
            new Claim { Id = "OVER", Customer = "B", Type = "Auto", PayableCents = ApprovalPolicy.AdjusterLimitCents + 1 },
        });

        Assert.Equal(ClaimStatus.Approved, queue.Approve("AT"));
        Assert.Equal(ClaimStatus.SentToSupervisor, queue.Approve("OVER"));
        Assert.Empty(queue.Pending);
    }
}
