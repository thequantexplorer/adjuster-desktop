using AdjusterDesktop.Core;
using Xunit;

public class ClaimQueueTests
{
    [Fact]
    public void ApprovingASmallClaimApprovesIt()
    {
        var queue = ClaimQueue.Sample();
        Assert.Equal(ClaimStatus.Approved, queue.Approve("CLM-1002"));
        Assert.DoesNotContain(queue.Pending, c => c.Id == "CLM-1002");
    }
}
