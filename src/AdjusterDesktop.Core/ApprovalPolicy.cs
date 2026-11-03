namespace AdjusterDesktop.Core;

/// <summary>
/// Adjusters can approve claims up to their authority limit.
/// A claim above the limit goes to a supervisor instead of being approved.
/// </summary>
public static class ApprovalPolicy
{
    public const long AdjusterLimitCents = 1_000_000; // $10,000

    public static ClaimStatus Decide(Claim claim, long limitCents = AdjusterLimitCents)
    {
        return claim.PayableCents <= limitCents ? ClaimStatus.Approved : ClaimStatus.SentToSupervisor;
    }
}
