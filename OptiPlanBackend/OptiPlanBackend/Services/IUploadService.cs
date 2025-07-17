namespace OptiPlanBackend.Services
{
    public interface IUploadService
    {
        Task<string?> UploadImageAsync(IFormFile file, string subFolder = "avatars");
    }

}
