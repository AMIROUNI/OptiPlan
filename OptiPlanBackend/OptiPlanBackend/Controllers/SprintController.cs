using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OptiPlanBackend.Dtos;
using OptiPlanBackend.Enums;
using OptiPlanBackend.Services.Interfaces;

namespace OptiPlanBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SprintController : ControllerBase
    {
        private readonly ISprintService sprintService;
        private readonly ILogger<SprintController> _logger;
        private readonly ICurrentUserService _currentUserService;
        private readonly IProjectService _projectService;
        private readonly ITeamService _teamService;

        public SprintController(ISprintService sprintService, ILogger<SprintController> logger,
            ICurrentUserService currentUserService, IProjectService projectService, ITeamService teamService)
        {
            this.sprintService = sprintService;
            _logger = logger;
            _currentUserService = currentUserService;
            _projectService = projectService;
            _teamService = teamService;
        }

        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> getSpritsByProjectId(Guid projectId)
        {
            if (!_currentUserService.UserId.HasValue)
            {
                _logger.LogWarning("Unauthorized access attempt to get sprints by project ID.");
                return Unauthorized("User is not authenticated.");
            }

            try
            {
                var projectSprints = await sprintService.GetSprintsByProjectId(projectId);
                return Ok(projectSprints);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving sprints for project ID {ProjectId}", projectId);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving sprints.");
            }
        }

        [HttpPost("project/{projectId}")]
        public async Task<IActionResult> CreateSprintForProject(Guid projectId, [FromBody] SprintDto sprintDto)
        {
            if (!_currentUserService.UserId.HasValue)
                return Unauthorized("User is not authenticated.");

            var userId = _currentUserService.UserId.Value;

            var project = await _projectService.GetByIdAsync(projectId);
            if (project == null)
                return BadRequest("Project does not exist.");

            if (sprintDto == null)
                return BadRequest("Sprint data is required.");

            var userRole = await _teamService.GetUserRoleInProjectAsync(userId, projectId);
            _logger.LogWarning(userRole.ToString());
            if (userRole is null)
                return StatusCode(StatusCodes.Status403Forbidden, "You are not a member of this project.");

            if (userRole != TeamRole.ProjectCreator && userRole != TeamRole.ProjectManager && userRole != TeamRole.TeamLeader)
                return StatusCode(StatusCodes.Status403Forbidden, "Only leaders/managers can create sprints.");

            try
            {
                var createdSprint = await sprintService.CreateSprintForProject(projectId, sprintDto);
                return CreatedAtAction(nameof(getSpritsByProjectId), new { projectId = projectId }, createdSprint);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating sprint for project ID {ProjectId}", projectId);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the sprint.");
            }
        }
    }
}
