namespace OptiPlanBackend.Dto
{
    public class InitializeProfileDto
    {


        public string Bio { get; set; } = string.Empty;

        public string? Skills { get; set; }



        public string FullName { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public IFormFile? Avatar { get; set; }   
        public IFormFile? Background { get; set; } 
        public string? CompanyName { get; set; }
        public string? Department { get; set; }
        public string? Country { get; set; }
    }
}
