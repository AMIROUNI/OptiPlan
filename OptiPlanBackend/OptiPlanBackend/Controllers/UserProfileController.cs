using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
                    Country = user.Country

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
        public async Task<IActionResult> InitializeProfile([FromForm] InitializeProfileDto profileDto) 
        {
            try
            {
                var userGuid = _currentUserService.UserId.Value;
                if (userGuid == Guid.Empty)
                {
                    _logger.LogError("in this positon  *****************************************");
                    return BadRequest("Invalid user ID");
                }

                // Get or create profile
                var userProfile = await _userProfileService.GetUserByIdAsync(userGuid);
                var isNewProfile = false;

                if (userProfile == null)
                {
                    userProfile = new UserProfile
                    {
                        UserId = userGuid,
                        Bio = profileDto.Bio ?? string.Empty,
                        CreatedAt = DateTime.UtcNow
                    };
                    isNewProfile = true;
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(profileDto.Bio))
                        userProfile.Bio = profileDto.Bio;

                    userProfile.UpdatedAt = DateTime.UtcNow;
                }
                List<SkillDto> skillDtos = new();
                if (!string.IsNullOrWhiteSpace(profileDto.Skills))
                {
                    skillDtos = JsonSerializer.Deserialize<List<SkillDto>>(profileDto.Skills) ?? new();
                }


              

                    foreach (var skillDto in skillDtos)
                    userProfile.Skills.Add(new Skill
                        {
                            Name = skillDto.Name,
                            ProficiencyLevel = skillDto.ProficiencyLevel,
                            YearsExperience = skillDto.YearsExperience,
                            UserProfileId = userProfile.Id
                        });
                    
                

                if (isNewProfile)
                    await _userProfileService.CreateAsync(userProfile);
                else
                    await _userProfileService.UpdateAsync(userProfile);

                // Update User info
                var user = await _userService.GetUserByIdAsync(userGuid);
                if (user != null)
                {
                    if (!string.IsNullOrWhiteSpace(profileDto.FullName))
                        user.FullName = profileDto.FullName;

                    if (!string.IsNullOrWhiteSpace(profileDto.JobTitle))
                        user.JobTitle = profileDto.JobTitle;

                    if (!string.IsNullOrWhiteSpace(profileDto.PhoneNumber))
                        user.PhoneNumber = profileDto.PhoneNumber;

                    if (!string.IsNullOrWhiteSpace(profileDto.CompanyName))
                        user.CompanyName = profileDto.CompanyName;

                    if (!string.IsNullOrWhiteSpace(profileDto.Department))
                        user.Department = profileDto.Department;

                    if (!string.IsNullOrWhiteSpace(profileDto.Country))
                        user.Country = profileDto.Country;

                    // Upload Avatar if provided
                    if (profileDto.Avatar != null)
                    {
                        user.AvatarUrl = await _uploadService.UploadImageAsync(profileDto.Avatar, "avatars");
                    }

                    // Upload Background if provided
                    if (profileDto.Background != null)
                    {
                        user.BackGround = await _uploadService.UploadImageAsync(profileDto.Background, "backgrounds");
                    }

                    if (user.firstLogin)
                        user.firstLogin = false;

                    await _userService.UpdateAsync(user);
                }

                return Ok(new
                {
                    Message = "Profile initialized successfully",
                    Profile = new
                    {
                        user.FullName,
                        user.JobTitle,
                        user.PhoneNumber,
                        user.AvatarUrl,
                        user.BackGround,
                        user.CompanyName,
                        user.Department,
                        user.Country,
                        userProfile.Bio,
                        Skills = userProfile.Skills
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initializing profile for user {UserId}", _currentUserService.UserId);
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
