namespace OptiPlanBackend.Services.Interfaces
{
    public interface IUploadService
    {
        Task<string?> UploadImageAsync(IFormFile file, string subFolder = "avatars");
    }

}
