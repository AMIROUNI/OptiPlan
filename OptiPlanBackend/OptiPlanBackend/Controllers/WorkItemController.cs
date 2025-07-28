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
    public class WorkItemController : ControllerBase
    {

        private readonly IWorkItemService _workItemService;
        private readonly ILogger<WorkItemController> _logger;
        private readonly ICurrentUserService _currentUserService;
        private readonly IProjectService _projectService;
        public WorkItemController(IWorkItemService taskService, ILogger<WorkItemController> logger, ICurrentUserService currentUserService,
            IProjectService projectService)
        {
            _workItemService = taskService;
            _logger = logger;
            _currentUserService = currentUserService;
            _projectService = projectService;
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var tasks = await _workItemService.GetAllAsync();
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
                var task = await _workItemService.GetByIdAsync(id);
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
                var tasks = await _workItemService.GetProjectTasksByProjectIdAsync(projectId);
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
            if (project == null)
            {
                return BadRequest("Project does not exisit");

            }
            if (projectTaskDto == null)
            {
                return BadRequest("Project task data is required");
            }
            try
            {

                _logger.LogInformation($"ReporterId: {_currentUserService.UserId.Value}");
                projectTaskDto.ProjectId = projectId;

                var createdTask = await _workItemService.AddWorkItemForAProject(projectTaskDto, _currentUserService.UserId.Value);
                return CreatedAtAction(nameof(GetById), new { id = createdTask.Id }, createdTask);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating task");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }



        [HttpPut("update-status")]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateWorkItemStatusDto dto)
        {
            try
            {
                var success = await _workItemService.UpdateWorkItemStatusAsync(dto.WorkItemId, dto.NewStatus);
                if (!success)
                    return NotFound(new { message = "Work item not found." });

                return Ok(new { message = "Status updated successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating work item status");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }

        }


        [HttpDelete("delete/{workItemId}")]
        [Authorize]
        public async Task<IActionResult> DeleteWorkItem(Guid workItemId)
        {
            try
            {
                var workItem = await _workItemService.GetByIdAsync(workItemId);
                if (workItem == null)
                {
                    return NotFound($"No work item with ID {workItemId}");
                }

                await _workItemService.DeleteAsync(workItem);
                return Ok(true);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Internal server error while deleting work item.");
                return StatusCode(500, false);
            }
        }



    }
}
