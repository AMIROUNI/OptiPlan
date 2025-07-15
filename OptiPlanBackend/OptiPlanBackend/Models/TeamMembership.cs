using OptiPlanBackend.Enums;

namespace OptiPlanBackend.Models
{
    public class TeamMembership
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid TeamId { get; set; }
        public Team Team { get; set; }

        public TeamRole Role { get; set; } = TeamRole.TeamMember;
        public MembershipStatus Status { get; set; } = MembershipStatus.Pending;
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    }
}