using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OptiPlanBackend.Models;
using OptiPlanBackend.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OptiPlanBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService userService) : ControllerBase
    {
        [HttpGet("profile")]
        public async Task<IActionResult> GetUserProfile()
        {
            try
            {
                // Get the authorization header
                var authHeader = Request.Headers["Authorization"].FirstOrDefault();

                if (string.IsNullOrEmpty(authHeader))
                {
                    return Unauthorized("Authorization header is missing");
                }
               
                var user = await userService.GetUserByTokenAsync(authHeader);

                if (user == null)
                {
                    return NotFound("User not found");
                }

                // Return the user profile (DTO recommended)
                return Ok(new
                {
                    user.Id,
                    user.Username,
                    user.Email,
                    user.AvatarUrl,
                    user.CompanyName,
                    user.PhoneNumber,
                    user.FullName,

                });
            }
            catch (Exception ex)
            {
                // Log the full exception
                Console.WriteLine($"Error in GetUserProfile: {ex}");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }




        [HttpGet("team/{projectId}")]
        [Authorize]
        public async Task<IActionResult> GetTeamByProjectId(Guid projectId)
        {
            try
            {
                var team = await userService.GetTeamByProjectId(projectId);
                if (team == null || !team.Any())
                {
                    return NotFound("No team members found for this project");
                }
                return Ok(team);
            }
            catch (Exception ex)
            {
                // Log the full exception
                Console.WriteLine($"Error in GetTeamByProjectId: {ex}");
                return StatusCode(500, "An error occurred while processing your request");
            }
        }



        [HttpGet("get-all-with-out-admins")]
        [Authorize]
        public async Task<IActionResult> GetAllWithOutAdmins()
        {
            try
            {
                var users = await  userService.getAllUserNotADMIN();
                return Ok(users);

            }

            catch(Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }




    }
}