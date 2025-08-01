using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OptiPlanBackend.Dto;
using OptiPlanBackend.Enums;
using OptiPlanBackend.Models;
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
        private readonly ITeamService _teamService;
        private readonly ITeamMembershipService _teamMembershipService;

        public ProjectController(IProjectService projectService, ICurrentUserService currentUserService, ILogger<ProjectController> logger ,
          ITeamService teamService,
           ITeamMembershipService teamMembershipService)
        {
            _projectService = projectService;
            _currentUserService = currentUserService;
            _logger = (ILogger<ProjectController>)logger;
            _teamService = teamService;
            _teamMembershipService = teamMembershipService;

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
               
                // 1. Créer le projet
                var createdProject = await _projectService.CreateProjectAsync(projectDto, _currentUserService.UserId.Value);

                // 2. Créer l'équipe associée au projet
                var team = new Team
                {
                    Id = Guid.NewGuid(),
                    Name = createdProject.Title, // même nom que le projet
                    ProjectId = createdProject.Id,
                    CreatedAt = DateTime.UtcNow
                };

                var teamCreated = await _teamService.CreateAsync(team);
                if (!teamCreated)
                {
                    return StatusCode(500, "Team creation failed");
                }

                // 3. Ajouter le créateur du projet comme membre de l'équipe avec rôle ProjectCreator
                var teamMembership = new TeamMembership
                {
                    Id = Guid.NewGuid(),
                    UserId = _currentUserService.UserId.Value,
                    TeamId = team.Id,
                    Role = TeamRole.ProjectCreator,
                    JoinedAt = DateTime.UtcNow
                };

                var memberAdded = await _teamMembershipService.CreateAsync(teamMembership);
                if (!memberAdded)
                {
                    return StatusCode(500, "Failed to assign user to team");
                }

                // 4. Retourner le résultat
                return CreatedAtAction(nameof(GetById), new { id = createdProject.Id }, createdProject);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a project and assigning team");
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



        [HttpGet("get-team/{projectId}")]
        public async Task<IActionResult> GetTeam(Guid projectId)
        {
            try
            {
                var team = await _projectService.GetTeamByProjectId(projectId);
                if (team == null)
                    return BadRequest("this team is not exsite");
                return Ok(team);

            }
            catch(Exception e) {
                _logger.LogError($"{e.Message}");
                 return StatusCode(500,e.Message);
            
            }
        }


    


     [HttpGet("get-team-memberships/{projectId}")]
        public async Task<IActionResult> GetTeamMemberShips(Guid projectId)
        {
            try
            {
                var teamMembreships = await _projectService.GetTeamMembershipsByProjectIdAsync(projectId);
                return Ok(teamMembreships);

            }
            catch (Exception e)
            {
                _logger.LogError($"{e.Message}");
                return StatusCode(500, e.Message);

            }
        }


    }

}