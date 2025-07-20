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

        // Relationships
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }

        public Guid? AssignedUserId { get; set; }
        public User? AssignedUser { get; set; }

        public Guid? ReporterId { get; set; }  // Who created the task
        public User? Reporter { get; set; }

        // Task Metadata
        public OptiPlanBackend.Enums.TaskStatus Status { get; set; } = OptiPlanBackend.Enums.TaskStatus.ToDo;
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;
        public TaskType Type { get; set; } = TaskType.Task;

        // Dates
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DueDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? CompletedAt { get; set; }

        // Progress Tracking
        public int EstimatedHours { get; set; }  // e.g., 2h, 8h
        public int? StoryPoints { get; set; }    // e.g., 1, 2, 3, 5 (Fibonacci)
        public double CompletionPercentage { get; set; } = 0;

        // Additional Fields
        public string? Labels { get; set; }       // Comma-separated: "bug,ui,backend"
        public string? SprintId { get; set; }    // If using sprints
        public bool IsBlocked { get; set; } = false;
        public string? BlockReason { get; set; }

        // Navigation
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
        public ICollection<TaskHistory> History { get; set; } = new List<TaskHistory>();
        public bool IsCompleted { get; internal set; }
    }
}