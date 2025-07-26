using OptiPlanBackend.Enums;
using System.ComponentModel.DataAnnotations;
using WorkItemStatus = OptiPlanBackend.Enums.WorkItemStatus;

namespace OptiPlanBackend.Dto
{
    public class ProjectTaskDto
    {

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        [Required]
        public Guid ProjectId { get; set; } 

        public Guid? AssignedUserId { get; set; } 
        public Guid? ReporterId { get; set; } 

        public WorkItemStatus Status { get; set; } = WorkItemStatus.ToDo;
        public WorkItemPriority Priority { get; set; } = WorkItemPriority.Medium;
        public WorkItemType Type { get; set; } = WorkItemType.Task;

        public DateTime? DueDate { get; set; }
        public DateTime? StartDate { get; set; }

        public int EstimatedHours { get; set; }
        public int? StoryPoints { get; set; }
        public double CompletionPercentage { get; set; } = 0;

        public string? Labels { get; set; }

        public bool IsBlocked { get; set; } = false;
        public string? BlockReason { get; set; }

        public Guid? SprintId { get; set; }
    }
}
