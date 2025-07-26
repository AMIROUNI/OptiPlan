using System.ComponentModel.DataAnnotations;

namespace OptiPlanBackend.Models
{
    public class Sprint
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;


        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public bool IsCompleted { get; set; } = false;

        // Foreign key
        public Guid ProjectId { get; set; }
        public Project Project{ get; set; }

        // Navigation
        public ICollection<WorkItem> Tasks { get; set; } = new List<WorkItem>();
    }
}
