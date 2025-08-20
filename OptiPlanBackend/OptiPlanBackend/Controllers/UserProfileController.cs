using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using OptiPlanBackend.Dto;
using OptiPlanBackend.Models;
using OptiPlanBackend.Services.Implementations;
using OptiPlanBackend.Services.Interfaces;
using System.Security.Claims;
using System.Text.Json;

namespace OptiPlanBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {

        private readonly IUserProfileService _userProfileService;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<UserProfileController> _logger;
        private readonly IUploadService _uploadService;
        private readonly IUserService _userService;

        public UserProfileController(IUserProfileService userProfileService,
           ICurrentUserService currentUserService , ILogger<UserProfileController> logger
            ,IUserService userService,
           IUploadService uploadService)
        {
            _userProfileService = userProfileService;
            _currentUserService = currentUserService;   
            _logger = logger;
            _userService = userService;
            _uploadService = uploadService;
        }



        [HttpGet("{username}")]
        [Authorize]
        public async Task<IActionResult> GetProfile(string username) {

            try
            { 
                if(string.IsNullOrEmpty(username))
                {
                    return BadRequest("Username cannot be null or empty");
                }
                var user = await _userService.GetUserByUsernameAsync(username);


                var userProfile = await _userProfileService.GetUserByIdAsync(user.Id);


                var userProfileDto = new UserProfileDto
                {
                    Bio = userProfile.Bio,
                    Skills = userProfile.Skills.Select(s => new SkillDto
                    {
                        Id = s.Id,
                        Name = s.Name,
                        ProficiencyLevel = s.ProficiencyLevel,
                        YearsExperience = s.YearsExperience
                    }).ToList(),

                    FullName = user.FullName,
                    JobTitle = user.JobTitle,
                    PhoneNumber = user.PhoneNumber,
                    AvatarUrl = user.AvatarUrl,
                    CompanyName = user.CompanyName,
                    Department = user.Department,
                    Country = user.Country,
                    BackGround = user.BackGround,
                    UserId = user.Id


                };

                return Ok(userProfileDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the user profile.");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }


        }


        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] UserProfileDto userProfile)
        {
            if (userProfile == null)
            {
                return BadRequest("User profile cannot be null");
            }
            try
            {
                var userId =_currentUserService.UserId.Value;
                if (userId == Guid.Empty)
                {
                    return BadRequest("User ID cannot be empty");
                }

                var userProfileToUpdate = await _userProfileService.GetUserByIdAsync(userId);
                userProfileToUpdate.Bio = userProfile.Bio;
                userProfileToUpdate.UpdatedAt= DateTime.UtcNow;
                var result = await _userProfileService.UpdateAsync(userProfileToUpdate);
                var UpdatedUser = await _userService.GetUserByIdAsync(userId);
                UpdatedUser.FullName = userProfile.FullName;
                UpdatedUser.PhoneNumber = userProfile.PhoneNumber;
                UpdatedUser.AvatarUrl = userProfile.AvatarUrl;
                UpdatedUser.Department = userProfile.Department;
                UpdatedUser.CompanyName = userProfile.CompanyName;
                UpdatedUser.Country = userProfile.Country;
                UpdatedUser.JobTitle = userProfile.JobTitle;
               var  result2 = await _userService.UpdateAsync(UpdatedUser);

                if (!(result && result2))
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error updating user profile");
                }
                return Ok(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the user profile.");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }




        [HttpPost("initialize-profile")]
        [Authorize]
        public async Task<IActionResult> InitializeProfile([FromForm] IFormCollection form)
        {
            var bio = form["Bio"].ToString();
            var avatar = form.Files.GetFile("Avatar");
            var background = form.Files.GetFile("Background");

         

            var profileDto = new InitializeProfileDto
            {
                Bio = bio,
                Avatar = avatar,
                Background = background,
         
            };

            var userId = _currentUserService.UserId;
            var user = await _userProfileService.InitializeProfileAsync(userId.Value, profileDto);

            return Ok(new
            {
                Message = "Profile initialized successfully",
                User = new
                {
                    user.FullName,
                    user.JobTitle,
                    user.PhoneNumber,
                    user.AvatarUrl,
                    user.BackGround,
                    user.CompanyName,
                    user.Department,
                    user.Country,
                    user.firstLogin,
                    Bio = user.Profile?.Bio,
                    Skills = user.Profile?.Skills
                }
            });
        }


    }
}
