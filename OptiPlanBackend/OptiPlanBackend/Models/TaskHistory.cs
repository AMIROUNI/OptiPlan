namespace OptiPlanBackend.Models
{
    public class TaskHistory
    {
        public Guid Id { get; set; }
        public string FieldChanged { get; set; }  // e.g., "Status", "Assignee"
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;

        public Guid TaskId { get; set; }
        public Task Task { get; set; }

        public Guid ChangedById { get; set; }
        public User ChangedBy { get; set; }
    }
}
