using System.ComponentModel.DataAnnotations;

namespace OptiPlanBackend.Models
{
    public class Conversation
    {
        [Key]
        public Guid Id { get; set; }

        public string Title { get; set; } = "Default Conversation";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Relation à l'utilisateur
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        // Navigation
        public ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
    }

}
