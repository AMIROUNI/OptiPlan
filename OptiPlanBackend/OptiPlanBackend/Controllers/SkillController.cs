using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OptiPlanBackend.Dto;
using OptiPlanBackend.Models;
using OptiPlanBackend.Services.Interfaces;

namespace OptiPlanBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillController : ControllerBase
    {
        private readonly ISkillService _skillService;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<SkillController> _logger;
        private readonly IUserProfileService _userProfileService;
        public SkillController(ISkillService skillService, ICurrentUserService currentUserService, 
            ILogger<SkillController> logger,IUserProfileService userProfileService
            )
        {
            _skillService = skillService;
            _currentUserService = currentUserService;
            _logger = logger;
            _userProfileService = userProfileService;
        }






        [HttpGet("user-skills")]
        [Authorize]
        public IActionResult Get()
        {
            try
            {
                var userId = _currentUserService.UserId.Value;
                if (userId == Guid.Empty)
                {
                    return BadRequest("User ID cannot be empty");
                }
                var skills = _skillService.GetUserSkillsByUserIdAsync(userId).Result;
                return Ok(skills);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting user skills.");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }

        }


        [HttpPost("add-skill")]
        [Authorize]
        public async Task<IActionResult> AddSkill([FromBody] SkillDto skillDto)
        {
            if (skillDto == null)
            {
                return BadRequest("Skill data cannot be null");
            }

            try
            {
                var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException("User not authenticated");
                if (userId == Guid.Empty)
                {
                    return BadRequest("Invalid user ID");
                }

                // Get user profile
                var userProfile = await _userProfileService.GetUserByIdAsync(userId);
                if (userProfile == null)
                {
                    return NotFound("User profile not found");
                }

                // Check for duplicate skill
                if (userProfile.Skills.Any(s =>
                    s.Name.Equals(skillDto.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    return Conflict($"Skill '{skillDto.Name}' already exists for this user");
                }

                // Create new skill
                var skill = new Skill
                {
                    Name = skillDto.Name,
        
                    ProficiencyLevel = skillDto.ProficiencyLevel,
                    YearsExperience = skillDto.YearsExperience,
                    UserProfileId = userProfile.Id
                };

                // Add skill to profile
                userProfile.Skills.Add(skill);
                userProfile.UpdatedAt = DateTime.UtcNow;

                // Save changes
                await _userProfileService.UpdateAsync(userProfile);

                return Ok(true);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding skill for user {UserId}", _currentUserService.UserId);
                return StatusCode(500, "An error occurred while adding the skill");
            }
        }



        [HttpDelete("{skillID}")]
        [Authorize]
        public  async Task<IActionResult> Delete(Guid
             skillID)
        {
            try
            {
                var skill = await _skillService.GetByIdAsync(
                    skillID);
                if(skill == null)
                {
                    return NotFound("can not found this skill ");
                }

                await _skillService.DeleteAsync(skill);
                return Ok(true);

            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, ex.Message);

            }

                
        }

    }
}
