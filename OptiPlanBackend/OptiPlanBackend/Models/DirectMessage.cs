using System.ComponentModel.DataAnnotations;

namespace OptiPlanBackend.Models
{
    public class DirectMessage
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Content { get; set; } = string.Empty;

        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        public Guid DirectChatId { get; set; }
        public DirectChat DirectChat { get; set; } = null!;

        public Guid SenderId { get; set; }
        public User Sender { get; set; } = null!;
    }
}
