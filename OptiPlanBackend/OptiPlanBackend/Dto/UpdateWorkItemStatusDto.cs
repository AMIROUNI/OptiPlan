using OptiPlanBackend.Enums;
using System.ComponentModel.DataAnnotations;

namespace OptiPlanBackend.Dto
{
    public class UpdateWorkItemStatusDto
    {
        [Required]
        public Guid WorkItemId { get; set; }

        [Required]
        public WorkItemStatus NewStatus { get; set; }
    }
}
