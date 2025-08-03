using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OptiPlanBackend.Dto;
using OptiPlanBackend.Enums;
using OptiPlanBackend.Models;
using OptiPlanBackend.Services.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class WorkItemController : ControllerBase
{
    private readonly IWorkItemService _workItemService;
    private readonly ILogger<WorkItemController> _logger;
    private readonly ICurrentUserService _currentUserService;
    private readonly IProjectService _projectService;
    private readonly ITeamService _teamService;
    private readonly IWorkItemHistoryService _workItemHistoryService;
    private readonly IUserService _userService;

    public WorkItemController(
        IWorkItemService taskService,
        ILogger<WorkItemController> logger,
        ICurrentUserService currentUserService,
        IProjectService projectService,
        ITeamService teamService,
        IWorkItemHistoryService workItemHistoryService,
          IUserService userService)
  
    {
        _workItemService = taskService;
        _logger = logger;
        _currentUserService = currentUserService;
        _projectService = projectService;
        _teamService = teamService;
        _workItemHistoryService = workItemHistoryService;
        _userService = userService;
    }

    // Méthodes publiques accessibles à tous les membres authentifiés
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
            if (task == null) return NotFound();
            return Ok(task);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching task by ID");
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }

    // Récupérer les tâches d’un projet — accessible uniquement aux membres du projet
    [HttpGet("get-by-project-id/{projectId}")]
    [Authorize]
    public async Task<IActionResult> GetByProjectId(Guid projectId)
    {

    if  (!_currentUserService.UserId.HasValue)
            return Unauthorized("User is not authenticated");

        var userId = _currentUserService.UserId.Value;

        // Vérifier que l'utilisateur appartient à ce projet 
        var userRole = await _teamService.GetUserRoleInProjectAsync(userId, projectId);
      /*  _logger.LogWarning(userRole.ToString());
        if (userRole.ToString() == null)
        {
            return StatusCode(StatusCodes.Status403Forbidden, "You are not a member of this project.");
        }*/

        try
        {
            var tasks = await _workItemService.GetWorkItemByProjectIdAsync(projectId);
            return Ok(tasks);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching tasks by project ID");
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }

    // Créer une tâche — uniquement pour ProjectCreator, ProjectManager, TeamLeader
    [HttpPost("Create/{projectId}")]
    public async Task<IActionResult> Create([FromBody] ProjectTaskDto projectTaskDto, Guid projectId)
    {
        if (!_currentUserService.UserId.HasValue)
            return Unauthorized("User is not authenticated");

        var userId = _currentUserService.UserId.Value;
        var project = await _projectService.GetByIdAsync(projectId);
        if (project == null) return BadRequest("Project does not exist");
        if (projectTaskDto == null) return BadRequest("Project task data is required");

        var userRole = await _teamService.GetUserRoleInProjectAsync(userId, projectId);
        if (userRole == null ||
            !(userRole == TeamRole.ProjectCreator || userRole == TeamRole.ProjectManager || userRole == TeamRole.TeamLeader))
            return StatusCode(StatusCodes.Status403Forbidden, "You do not have permission to create tasks.");

        try
        {
            projectTaskDto.ProjectId = projectId;
            var createdTask = await _workItemService.AddWorkItemForAProject(projectTaskDto, userId);

            // Ajouter l’historique
            var history = new WorkItemHistory
            {
                FieldChanged = "WorkItem Created",
                OldValue = "",
                NewValue = $"Title: {createdTask.Title}, Status: {createdTask.Status}, AssignedUser: {createdTask.AssignedUserId}",
                ChangedAt = DateTime.UtcNow,
                ChangedById = userId,
                WorkItemId = createdTask.Id
            };
            await _workItemHistoryService.CreateAsync(history);

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
        if (!_currentUserService.UserId.HasValue)
            return Unauthorized("User is not authenticated");

        var userId = _currentUserService.UserId.Value;
        var workItem = await _workItemService.GetByIdAsync(dto.WorkItemId);
        if (workItem == null) return NotFound("Work item not found.");

        var userRole = await _teamService.GetUserRoleInProjectAsync(userId, workItem.ProjectId);
        if (userRole == null || !(userRole == TeamRole.ProjectCreator || userRole == TeamRole.ProjectManager || userRole == TeamRole.TeamLeader))
            return StatusCode(StatusCodes.Status403Forbidden, "You do not have permission to update task status.");

        try
        {
            var oldStatus = workItem.Status.ToString();
            var success = await _workItemService.UpdateWorkItemStatusAsync(dto.WorkItemId, dto.NewStatus);

            if (!success) return NotFound("Work item not found.");

            var history = new WorkItemHistory
            {
                FieldChanged = "Status",
                OldValue = oldStatus,
                NewValue = dto.NewStatus.ToString(),
                ChangedAt = DateTime.UtcNow,
                ChangedById = userId,
                WorkItemId = workItem.Id
            };

            await _workItemHistoryService.CreateAsync(history);
            return Ok(new { message = "Status updated successfully." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating work item status");
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }


    // Supprimer une tâche — uniquement pour ProjectCreator, ProjectManager
    [HttpDelete("delete/{workItemId}")]
    [Authorize]
    public async Task<IActionResult> DeleteWorkItem(Guid workItemId)
    {
        if (!_currentUserService.UserId.HasValue)
            return Unauthorized("User is not authenticated");

        var userId = _currentUserService.UserId.Value;

        var workItem = await _workItemService.GetByIdAsync(workItemId);
        if (workItem == null)
            return NotFound($"No work item with ID {workItemId}");

        // Vérifier rôle autorisé
        var userRole = await _teamService.GetUserRoleInProjectAsync(userId, workItem.ProjectId);
        if (userRole == null)
            return StatusCode(StatusCodes.Status403Forbidden, "You are not a member of this project.");

        if (userRole != TeamRole.ProjectCreator &&
            userRole != TeamRole.ProjectManager)
        {
            return StatusCode(StatusCodes.Status403Forbidden, "You do not have permission to delete tasks.");
        }

        try
        {
            await _workItemService.DeleteAsync(workItem);
            return Ok(true);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Internal server error while deleting work item.");
            return StatusCode(500, false);
        }
    }


    [HttpPut("assign-user/{workItemId}/{userID}")]
    [Authorize]
    public async Task<IActionResult> AssignUserToWorkItem(Guid workItemId, Guid userID)
    {

       
        if (!_currentUserService.UserId.HasValue)
            return Unauthorized("User is not authenticated");

        var currentUserId = _currentUserService.UserId.Value;
        var workItem = await _workItemService.GetByIdAsync(workItemId);
        if (workItem == null)
            return NotFound($"No work item with ID {workItemId}");

        var user = await _userService.GetUserByIdAsync(userID);
        if (user == null)
            return NotFound($"User with ID {userID} does not exist.");

        var userRole = await _teamService.GetUserRoleInProjectAsync(currentUserId, workItem.ProjectId);
        if (userRole == null || !(userRole == TeamRole.ProjectCreator || userRole == TeamRole.ProjectManager || userRole == TeamRole.TeamLeader))
            return StatusCode(StatusCodes.Status403Forbidden, "You do not have permission to assign users.");

        try
        {
            var history = new WorkItemHistory
            {
                FieldChanged = "AssignedUser",
                OldValue = workItem.AssignedUserId?.ToString() ?? "null",
                NewValue = userID.ToString(),
                WorkItemId = workItem.Id,
                ChangedAt = DateTime.UtcNow,
                ChangedById = currentUserId,
            };

            workItem.AssignedUserId = userID;
            workItem.AssignedUser = user;
            await _workItemService.UpdateAsync(workItem);
            await _workItemHistoryService.CreateAsync(history);

            return Ok(workItem);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Internal server error while assigning user to work item.");
            return StatusCode(500, "Internal error");
        }
    }


}
