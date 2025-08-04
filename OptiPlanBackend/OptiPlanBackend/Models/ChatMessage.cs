using System.ComponentModel.DataAnnotations;

namespace OptiPlanBackend.Models
{
    public class ChatMessage
    {
        [Key]
        public Guid Id { get; set; }

        public string Content { get; set; } = string.Empty;
        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        // Type : "user" ou "assistant"
        public string Role { get; set; } = "user"; // ou "assistant"

        // Relation avec la conversation
        public Guid ConversationId { get; set; }
        public Conversation Conversation { get; set; } = null!;
    }

}
