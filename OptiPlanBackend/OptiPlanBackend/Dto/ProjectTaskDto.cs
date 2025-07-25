using OptiPlanBackend.Enums;
using System.ComponentModel.DataAnnotations;
using TaskStatus = OptiPlanBackend.Enums.TaskStatus;

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

        public TaskStatus Status { get; set; } = TaskStatus.ToDo;
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;
        public TaskType Type { get; set; } = TaskType.Task;

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
