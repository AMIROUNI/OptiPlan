namespace OptiPlanBackend.Models
{
    public class Project
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Guid OwnerId { get; set; }
        public User Owner { get; set; }

        public Team Team { get; set; }

        public ICollection<WorkItem> Tasks { get; set; } = new List<WorkItem>();

     
        public ICollection<Sprint> Sprints { get; set; } = new List<Sprint>();
    }
}
