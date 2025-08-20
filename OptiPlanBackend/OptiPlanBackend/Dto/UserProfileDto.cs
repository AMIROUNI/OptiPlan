using OptiPlanBackend.Models;

namespace OptiPlanBackend.Dto
{
    public class UserProfileDto
    {

        public string Bio { get; set; } = string.Empty;
       
        public List<SkillDto>? Skills { get; set; }
        public string? BackGround{ get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string AvatarUrl { get; set; }  
      
        public string? CompanyName { get; set; }
        public string? Department { get; set; }
        public string? Country { get; set; }
        public Guid? UserId { get; set; }

    }
}
