namespace AdjusterDesktop.Core;

/// <summary>
/// Adjusters can approve claims up to their authority limit for the claim type.
/// A claim above the limit goes to a supervisor instead of being approved.
/// </summary>
public static class ApprovalPolicy
{
    public const long AdjusterLimitCents = 1_000_000; // $10,000

    /// <summary>Authority limit per claim type, in cents. Theft claims have a lower limit.</summary>
    public static readonly IReadOnlyDictionary<string, long> TypeLimitsCents = new Dictionary<string, long>
    {
        ["Auto"] = AdjusterLimitCents,
        ["Property"] = AdjusterLimitCents,
        ["Theft"] = 500_000, // $5,000
    };

    public static ClaimStatus Decide(Claim claim) => Decide(claim, TypeLimitsCents[claim.Type]);

    public static ClaimStatus Decide(Claim claim, long limitCents)
    {
        return claim.PayableCents <= limitCents ? ClaimStatus.Approved : ClaimStatus.SentToSupervisor;
    }
}
