using System.Text.Json.Serialization;

namespace OptiPlanBackend.Models
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Guid WorkItemId { get; set; }


        [JsonIgnore]
        public WorkItem? WorkItem { get; set; }

        public Guid AuthorId { get; set; }
        [JsonIgnore]
        public User? Author { get; set; }
    }
}
