using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OptiPlanBackend.Models;
using OptiPlanBackend.Services.Interfaces;

namespace OptiPlanBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectTaskController : ControllerBase
    {

        private readonly ITaskService _taskService;
        private readonly ILogger<ProjectTaskController> _logger;
        private readonly ICurrentUserService _currentUserService;
        private readonly IProjectService _projectService;
        public ProjectTaskController(ITaskService taskService, ILogger<ProjectTaskController> logger, ICurrentUserService currentUserService,
            IProjectService projectService)
        {
            _taskService = taskService;
            _logger = logger;
            _currentUserService = currentUserService;
            _projectService = projectService;
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var tasks = await _taskService.GetAllAsync();
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all tasks");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {


            try
            {
                var task = await _taskService.GetByIdAsync(id);
                if (task == null)
                {
                    return NotFound();
                }
                return Ok(task);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching task by ID");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }



        [HttpGet("get-by-project-id/{projectId}")]
        public async Task<IActionResult> GetByProjectId(Guid projectId)
        { 

            if (!_currentUserService.UserId.HasValue)
            {
                return Unauthorized("User is not authenticated");
            }
            try
            {
                var tasks = await _taskService.GetProjectTasksByProjectIdAsync(projectId);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching tasks by project ID");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPost("Create/{projectId}")]
        public async Task<IActionResult> Create([FromBody] Dto.ProjectTaskDto projectTaskDto,
            Guid projectId)
        {
            if (!_currentUserService.UserId.HasValue)
            {
                return Unauthorized("User is not authenticated");
            }
            var project = await _projectService.GetByIdAsync(projectId);
            if(project == null)
            {
                return BadRequest("Project does not exisit");

            }
            if( projectTaskDto == null)
            {
                return BadRequest("Project task data is required");
            }
            try
            {

               _logger.LogInformation($"ReporterId: {_currentUserService.UserId.Value}");
                projectTaskDto.ProjectId = projectId;

                var createdTask = await _taskService.AddProjectTaskForAProject(projectTaskDto, _currentUserService.UserId.Value);
                return CreatedAtAction(nameof(GetById), new { id = createdTask.Id }, createdTask);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating task");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

    }
}
