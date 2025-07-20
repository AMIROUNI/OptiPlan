using Microsoft.AspNetCore.Mvc;
using OptiPlanBackend.Services.Interfaces;

namespace OptiPlanBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<ProjectController> _logger;
        private readonly ITaskService _taskService;

        public DashboardController(
            IDashboardService dashboardService,
            ICurrentUserService currentUserService,
            ILogger<ProjectController> logger,
            ITaskService taskService)

        {
            _dashboardService = dashboardService ?? throw new ArgumentNullException(nameof(dashboardService));
        _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        _logger = logger;

          _taskService= taskService;
        }

        [HttpGet("kpis")]
        public async Task<IActionResult> GetUserKpis()
        {
            if (!_currentUserService.UserId.HasValue)
            {
                return Unauthorized("User not authenticated");
            }

            var kpis = await _dashboardService.GetUserKpis(_currentUserService.UserId.Value);
            return Ok(kpis);
        }



        [HttpGet("tasks-grouped-by-project-for-month")]
        public async Task<IActionResult> GetUserTasksGroupedByProjectForMonth(
     [FromQuery] int month,
     [FromQuery] int year)
        {
            if (!_currentUserService.UserId.HasValue)
                return Unauthorized("User not authenticated");

            try
            {
                var userId = _currentUserService.UserId.Value;
                var groupedTasks = await _taskService.GetUserTasksGroupedByProjectForMonth(userId, month, year);

                return Ok(groupedTasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching tasks grouped by project");
                return StatusCode(500, "An error occurred while fetching tasks.");
            }
        }
    }

}