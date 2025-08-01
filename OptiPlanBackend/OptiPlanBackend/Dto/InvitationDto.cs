using OptiPlanBackend.Enums;
using OptiPlanBackend.Models;
using System.ComponentModel.DataAnnotations;

namespace OptiPlanBackend.Dto
{
    public class InvitationDto
    {
       

        [Required]
        public Guid TeamId { get; set; }

        // Invited user (if registered)
        public Guid? InviteeId { get; set; }
   

        // Email for non-registered users
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public TeamRole Role { get; set; } = TeamRole.Guest;


        [Required]
        public Guid InviterId { get; set; }
   

    }
}
