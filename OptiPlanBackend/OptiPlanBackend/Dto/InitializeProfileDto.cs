namespace OptiPlanBackend.Dto
{
    public class InitializeProfileDto
    {
        public string Bio { get; set; } = string.Empty;

        public IFormFile? Avatar { get; set; }
        public IFormFile? Background { get; set; }


    }
}
