using OptiPlanBackend.Enums;

namespace OptiPlanBackend.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public Role Role { get; set; } = Role.User;

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

        // Navigation properties
        public ICollection<Project> OwnedProjects { get; set; } = new List<Project>();
        public ICollection<TeamMembership> TeamMemberships { get; set; } = new List<TeamMembership>();
        public UserProfile Profile { get; set; }
    }
}