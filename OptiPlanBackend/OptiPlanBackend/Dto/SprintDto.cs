using System.ComponentModel.DataAnnotations;

namespace OptiPlanBackend.Dtos
{
    public class SprintDto
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; } 

        [Required]
        public DateTime EndDate { get; set; }
    }
}
