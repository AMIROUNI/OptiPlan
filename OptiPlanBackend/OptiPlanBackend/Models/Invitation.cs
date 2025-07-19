using OptiPlanBackend.Enums;
using System.ComponentModel.DataAnnotations;

namespace OptiPlanBackend.Models
{
    public class Invitation
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid TeamId { get; set; }
        public Team Team { get; set; }

        // Invited user (if registered)
        public Guid? InviteeId { get; set; }
        public User? Invitee { get; set; }

        // Email for non-registered users
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        // Invitation metadata
        public InvitationStatus Status { get; set; } = InvitationStatus.Pending;
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public DateTime? RespondedAt { get; set; }

        // Inviter (who sent the invite)
        [Required]
        public Guid InviterId { get; set; }
        public User Inviter { get; set; }

        // Old property replaced by Status enum
        [Obsolete("Use Status property instead")]
        public bool IsAccepted { get; set; } = false;
    }
}