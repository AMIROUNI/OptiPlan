

namespace OptiPlanBackend.Models
{
    public class UserProfile
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }

        public string Bio { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // One-to-Many relation
        public ICollection<Skill> Skills { get; set; } = new List<Skill>();
    }


}
