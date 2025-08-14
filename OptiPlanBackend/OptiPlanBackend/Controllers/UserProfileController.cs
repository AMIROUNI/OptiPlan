using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OptiPlanBackend.Dto;
using OptiPlanBackend.Models;
using OptiPlanBackend.Services.Implementations;
using OptiPlanBackend.Services.Interfaces;
using System.Security.Claims;

namespace OptiPlanBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {

        private readonly IUserProfileService _userProfileService;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<UserProfileController> _logger;
        private readonly IUserService _userService;

        public UserProfileController(IUserProfileService userProfileService,
           ICurrentUserService currentUserService , ILogger<UserProfileController> logger
            ,IUserService userService)
        {
            _userProfileService = userProfileService;
            _currentUserService = currentUserService;   
            _logger = logger;
            _userService = userService;
        }



        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetProfile() {

            try
            { 

                var userId = _currentUserService.UserId.Value;
                if (userId == Guid.Empty)
                {
                    return BadRequest("User ID cannot be empty");
                }

                var userProfile = await _userProfileService.GetUserByIdAsync(userId);
                var user = await _userService.GetUserByIdAsync(userId);

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
        public async Task<IActionResult> InitializeProfile([FromBody] UserProfileDto profileDto)
        {
            try
            {
                var userGuid = _currentUserService.UserId.Value;
                if (userGuid == Guid.Empty)
                {
                    return BadRequest("Invalid user ID");
                }

                // Get or create user profile
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
                    userProfile.Bio = profileDto.Bio ?? userProfile.Bio;
                    userProfile.UpdatedAt = DateTime.UtcNow;
                }

                // Process skills if provided
                if (profileDto.Skills != null && profileDto.Skills.Any())
                {
                    // Clear existing skills if this is not a new profile
                    if (!isNewProfile && userProfile.Skills.Any())
                    {
                        userProfile.Skills.Clear();
                    }

                    // Add new skills
                    foreach (var skillDto in profileDto.Skills)
                    {
                        var skill = new Skill
                        {
                            Name = skillDto.Name,
                            ProficiencyLevel = skillDto.ProficiencyLevel,
                            YearsExperience = skillDto.YearsExperience,
                            UserProfileId = userProfile.Id
                        };
                        userProfile.Skills.Add(skill);
                    }
                }

                // Save profile changes
                if (isNewProfile)
                {
                    await _userProfileService.CreateAsync(userProfile);
                }
                else
                {
                    await _userProfileService.UpdateAsync(userProfile);
                }

                // Update user's firstLogin status
                var user = await _userService.GetUserByIdAsync(userGuid);
                if (user != null)
                {
                    user.firstLogin = false;
                    await _userService.UpdateAsync(user);
                }

                return Ok(new
                {
                    Message = "Profile initialized successfully",
                    Profile = userProfile,
                    SkillsAdded = profileDto.Skills?.Count ?? 0
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
