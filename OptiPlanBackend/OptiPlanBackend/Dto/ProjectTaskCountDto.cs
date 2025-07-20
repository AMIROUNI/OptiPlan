namespace OptiPlanBackend.Dto
{
    public class ProjectTaskCountDto
    {
        public Guid ProjectId { get; set; }
        public string ProjectTitle { get; set; } = string.Empty;
        public int TaskCount { get; set; }
    }
}
