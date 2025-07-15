namespace OptiPlanBackend.Models
{
    public class Team
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public Guid ProjectId { get; set; }
        public Project Project { get; set; }

        public ICollection<TeamMembership> Members { get; set; } = new List<TeamMembership>();
        public ICollection<Invitation> Invitations { get; set; } = new List<Invitation>();
    }
}