
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class UploadController : ControllerBase
{
    private readonly IWebHostEnvironment _environment;

    public UploadController(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    [HttpPost("avatar")]
    public async Task<IActionResult> UploadAvatar([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        var uploadsFolder = Path.Combine(_environment.WebRootPath, "avatars");
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        var fileUrl = $"{baseUrl}/avatars/{uniqueFileName}";

        return Ok(new { avatarUrl = fileUrl });
    }
}
