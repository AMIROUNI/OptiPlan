namespace OptiPlanBackend.Services.Interfaces
{
    public interface ICurrentUserService
    {
        Guid? UserId { get; }
        string? Email { get; }
    }

}
