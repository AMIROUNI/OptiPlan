using OptiPlanBackend.Models;
using System.ComponentModel.DataAnnotations;

namespace OptiPlanBackend.Dto
{
    public class SkillDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Range(1, 5)]
        public int ProficiencyLevel { get; set; } = 1;

        [Range(0, 50)]
        public int YearsExperience { get; set; } = 0;
    }
}
