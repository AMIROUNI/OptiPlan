using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OptiPlanBackend.Dto;
using OptiPlanBackend.Services.Implementations;
using OptiPlanBackend.Services.Interfaces;

namespace OptiPlanBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {

        private readonly IProjectService _projectService;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<ProjectController> _logger;

        public ProjectController(IProjectService projectService, ICurrentUserService currentUserService, ILogger<ProjectController> logger)
        {
            _projectService = projectService;
            _currentUserService = currentUserService;
            _logger = (ILogger<ProjectController>)logger;
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var project = await _projectService.GetByIdAsync(id);

                if (project == null)
                {
                    _logger.LogWarning("Project with ID {ProjectId} not found.", id);
                    return NotFound();
                }

                _logger.LogInformation("Project with ID {ProjectId} retrieved successfully.", id);
                return Ok(project);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving project with ID {ProjectId}.", id);
                return StatusCode(500, "Internal server error");
            }
        }





        [HttpPost("user-create")]
        public async Task<IActionResult> Create(ProjectDto projectDto)
        {
            if (!_currentUserService.UserId.HasValue)
                return Unauthorized("User not authenticated");

            try
            {
                var createdProject = await _projectService.CreateProjectAsync(projectDto, _currentUserService.UserId.Value);

                return CreatedAtAction(nameof(GetById), new { id = createdProject.Id }, createdProject);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a project");
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpGet("get-projects-for-user")]
        public async Task<IActionResult> GetProjectsForUserAsync()
        {
            if (!_currentUserService.UserId.HasValue)
                return Unauthorized("User not authenticated");

            try
            {
                var projects = await _projectService.GetProjectsForUserAsync(_currentUserService.UserId.Value);
                return Ok(projects);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching user projects");
                return StatusCode(500, "Internal server error");
            }
        }


    }

}