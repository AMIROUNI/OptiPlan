using OptiPlanBackend.Enums;
using System.ComponentModel.DataAnnotations;

namespace OptiPlanBackend.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public Role Role { get; set; } = Role.User;


        public bool firstLogin { get; set; } = false;

       
        public string FullName { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
        public string? CompanyName { get; set; }
        public string? Department { get; set; }
        public string? Country { get; set; }

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

        // Navigation
        public ICollection<Project> OwnedProjects { get; set; } = new List<Project>();
        public ICollection<TeamMembership> TeamMemberships { get; set; } = new List<TeamMembership>();
        public UserProfile? Profile { get; set; }



        public ICollection<WorkItem> AssignedTasks { get; set; } = new List<WorkItem>();
        public ICollection<WorkItem> ReportedTasks { get; set; } = new List<WorkItem>();

        public ICollection<Invitation> SentInvitations { get; set; } = new List<Invitation>();  // As Inviter
        public ICollection<Invitation> ReceivedInvitations { get; set; } = new List<Invitation>();
        public ICollection<Conversation> Conversations { get; set; } = new List<Conversation>();
        // As Invitee
    }

}