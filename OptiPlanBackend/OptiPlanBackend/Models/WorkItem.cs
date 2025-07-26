
using OptiPlanBackend.Enums;
using System.ComponentModel.DataAnnotations;

namespace OptiPlanBackend.Models
{
    public class WorkItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }

        public WorkItemType Type { get; set; }  // Story, Bug, Task, Epic, Subtask
        public WorkItemStatus Status { get; set; }
        public WorkItemPriority Priority { get; set; }

        public Guid ProjectId { get; set; }
        public Project Project { get; set; }

        public Guid? SprintId { get; set; }
        public Sprint? Sprint { get; set; }

        public Guid? ParentId { get; set; }  // for nesting (Story → Subtask)
        public WorkItem? Parent { get; set; }

        public ICollection<WorkItem> SubItems { get; set; } = new List<WorkItem>(); // e.g. subtasks

        public Guid? AssignedUserId { get; set; }
        public User? AssignedUser { get; set; }

        public Guid? ReporterId { get; set; }
        public User? Reporter { get; set; }

        public DateTime? StartDate { get; set; }
        public int? EstimatedHours { get; set; }
        public string? Labels { get; set; } 



        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DueDate { get; set; }
        public DateTime? CompletedAt { get; set; }

        public int? StoryPoints { get; set; }  // Only for Story-type
        public double CompletionPercentage { get; set; } = 0;

        public bool IsBlocked { get; set; }
        public string? BlockReason { get; set; }

        public bool IsCompleted => Status == WorkItemStatus.Done;


        public ICollection<WorkItemHistory> History { get; set; } = new List<WorkItemHistory>();
    }

}
