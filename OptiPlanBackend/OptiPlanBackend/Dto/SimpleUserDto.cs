namespace OptiPlanBackend.Dto
{
    public class SimpleUserDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string AvatarUrl { get; set; }
        public string JobTitle { get; set; }
    }

}
