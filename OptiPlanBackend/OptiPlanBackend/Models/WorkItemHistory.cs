namespace OptiPlanBackend.Models
{
    public class WorkItemHistory
    {
        public Guid Id { get; set; }
        public string FieldChanged { get; set; }  // e.g., "Status", "Assignee"
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;

        public Guid WorkItemId { get; set; }  
        public WorkItem WorkItem { get; set; }

        public Guid ChangedById { get; set; }
        public User ChangedBy { get; set; }
    }
}
