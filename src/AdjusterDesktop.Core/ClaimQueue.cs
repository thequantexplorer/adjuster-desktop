namespace AdjusterDesktop.Core;

public sealed class ClaimQueue
{
    private readonly List<Claim> _claims;

    public ClaimQueue(IEnumerable<Claim> claims) => _claims = claims.ToList();

    public IReadOnlyList<Claim> Pending => _claims.Where(c => c.Status == ClaimStatus.InReview).ToList();
    public IReadOnlyList<Claim> All => _claims;

    public ClaimStatus Approve(string claimId)
    {
        var claim = _claims.Single(c => c.Id == claimId);
        claim.Status = ApprovalPolicy.Decide(claim);
        return claim.Status;
    }

    public static ClaimQueue Sample() => new(new[]
    {
        new Claim { Id = "CLM-1002", Customer = "Ana Ruiz", Type = "Auto", PayableCents = 270_000 },
        new Claim { Id = "CLM-1007", Customer = "Grace Liu", Type = "Property", PayableCents = 845_050 },
        new Claim { Id = "CLM-2001", Customer = "Ben Okafor", Type = "Theft", PayableCents = 2_000_000 },
        new Claim { Id = "CLM-2004", Customer = "Sam Patel", Type = "Water damage", PayableCents = 1_230_000 },
    });
}
