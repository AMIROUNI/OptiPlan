using OptiPlanBackend.Models;

namespace OptiPlanBackend.Dto
{
    public class UserProfileDto
    {

        public string Bio { get; set; } = string.Empty;
        public List<SkillDto>? Skills { get; set; }

    }
}
