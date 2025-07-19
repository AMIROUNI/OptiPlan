namespace OptiPlanBackend.Models
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Guid TaskId { get; set; }
        public Task Task { get; set; }

        public Guid AuthorId { get; set; }
        public User Author { get; set; }
    }
}
