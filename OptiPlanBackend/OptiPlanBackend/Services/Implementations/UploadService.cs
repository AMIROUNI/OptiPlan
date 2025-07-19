using Microsoft.AspNetCore.Hosting;
using OptiPlanBackend.Services.Interfaces;

public class UploadService : IUploadService
{
    private readonly IWebHostEnvironment _env;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UploadService(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
    {
        _env = env;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<string?> UploadImageAsync(IFormFile file, string subFolder = "avatars")
    {
        if (file == null || file.Length == 0)
            return null;

        var uploadsFolder = Path.Combine(_env.WebRootPath, subFolder);
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        var request = _httpContextAccessor.HttpContext!.Request;
        var baseUrl = $"{request.Scheme}://{request.Host}";
        return $"{baseUrl}/{subFolder}/{uniqueFileName}";
    }
}
