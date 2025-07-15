namespace OptiPlanBackend.Models
{
    public class Invitation
    {
        public Guid Id { get; set; }
        public Guid TeamId { get; set; }
        public Team Team { get; set; }
        public string Email { get; set; } = string.Empty;
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public bool IsAccepted { get; set; } = false;
    }
}