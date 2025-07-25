
using OptiPlanBackend.Enums;
using System.ComponentModel.DataAnnotations;

namespace OptiPlanBackend.Models
{
    public class ProjectTask
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        public Guid ProjectId { get; set; }
        public Project Project { get; set; }

        public Guid? AssignedUserId { get; set; }
        public User? AssignedUser { get; set; }

        public Guid? ReporterId { get; set; }
        public User? Reporter { get; set; }

        public OptiPlanBackend.Enums.TaskStatus Status { get; set; } = OptiPlanBackend.Enums.TaskStatus.ToDo;
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;
        public TaskType Type { get; set; } = TaskType.Task;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DueDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? CompletedAt { get; set; }

        public int EstimatedHours { get; set; }
        public int? StoryPoints { get; set; }
        public double CompletionPercentage { get; set; } = 0;

        public string? Labels { get; set; }
        public bool IsBlocked { get; set; } = false;
        public string? BlockReason { get; set; }

        // ✅ Sprint relationship
        public Guid? SprintId { get; set; }
        public Sprint? Sprint { get; set; }

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
        public ICollection<TaskHistory> History { get; set; } = new List<TaskHistory>();

        public bool IsCompleted { get; internal set; }
    }
}
