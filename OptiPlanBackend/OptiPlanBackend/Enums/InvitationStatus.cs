namespace OptiPlanBackend.Enums
{
    public enum InvitationStatus
    {
        Pending,    // Awaiting response
        Accepted,   // User joined
        Rejected,   // User declined
        Expired     // Auto-expired after X days
    }
}
