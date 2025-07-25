using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using OptiPlanBackend.Dtos;
using OptiPlanBackend.Services.Implementations;
using OptiPlanBackend.Services.Interfaces;
using System.Net.NetworkInformation;

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

        public SprintController(ISprintService sprintService, ILogger<SprintController> logger
            , ICurrentUserService currentUserService, IProjectService projectService)
        {
            this.sprintService = sprintService;
            _logger = logger;
            _currentUserService = currentUserService;
            _projectService = projectService;
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

                var rojectSprints = await sprintService.GetSprintsByProjectId(projectId);
                return Ok(rojectSprints);
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

            var project = await _projectService.GetByIdAsync(projectId);
            if (project == null)
            {
                _logger.LogWarning(" project does not exsit {ProjectId}.", projectId);
                return BadRequest(" project does not exsit");

            }
            if (!_currentUserService.UserId.HasValue)
            {
                _logger.LogWarning("Unauthorized access attempt to create sprint for project ID {ProjectId}.", projectId);
                return Unauthorized("User is not authenticated.");
            }
            if (sprintDto == null)
            {
                _logger.LogWarning("Invalid sprint data provided for project ID {ProjectId}.", projectId);
                return BadRequest("Sprint data is required.");
            }

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