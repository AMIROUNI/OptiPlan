using OptiPlanBackend.Models;
using OptiPlanBackend.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OptiPlanBackend.Services.Interfaces;

namespace OptiPlanBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService,IUploadService 
        uploadService,IUserProfileService  userProfileService) : ControllerBase
    {
        [HttpPost("register")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<User>> Register([FromForm] RegisterDto request, [FromForm] IFormFile? avatar)
        {
            try
            {
                if (avatar != null)
                {
                    var avatarUrl = await uploadService.UploadImageAsync(avatar, "avatars");
                    request.AvatarUrl = avatarUrl!;
                }

                var user = await authService.RegisterAsync(request);
                if (user is null)
                    return BadRequest("Username or email are  already exists.");


                var userProfile = new UserProfile
                {
                    UserId = user.Id,
                };
                await userProfileService.CreateAsync(userProfile);
                return Ok(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during registration: {ex.Message}");
                return StatusCode(500, "Internal server error during registration.");
            }
        }


        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDto>> Login(UserDto request)
        {
            Console.WriteLine($"Login attempt for username: {request.Username}");
            Console.WriteLine($"Request received at: {DateTime.UtcNow}");

            var result = await authService.LoginAsync(request);

            if (result is null)
            {
                Console.WriteLine("Login failed - invalid credentials");
                return BadRequest("Invalid username or password.");
            }

            Console.WriteLine($"Login successful for user: {request.Username}");
            Console.WriteLine($"Token generated: {result.AccessToken?.Substring(0, 10)}...");

            return Ok(result);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponseDto>> RefreshToken(RefreshTokenRequestDto request)
        {
            var result = await authService.RefreshTokensAsync(request);
            if (result is null || result.AccessToken is null || result.RefreshToken is null)
                return Unauthorized("Invalid refresh token.");

            return Ok(result);
        }

        [Authorize]
        [HttpGet]
        public IActionResult AuthenticatedOnlyEndpoint()
        {
            return Ok("You are authenticated!");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin-only")]
        public IActionResult AdminOnlyEndpoint()
        {
            return Ok("You are and admin!");
        }
    }
}
