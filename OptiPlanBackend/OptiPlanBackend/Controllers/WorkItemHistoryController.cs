using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OptiPlanBackend.Services.Interfaces;

namespace OptiPlanBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkItemHistoryController : ControllerBase
    {
        private readonly IWorkItemHistoryService _workItemHistoryService;
        private readonly ILogger<WorkItemHistoryController> _logger;
        private  readonly IWorkItemService _workItemService;

        public WorkItemHistoryController(IWorkItemHistoryService workItemHistoryService, ILogger<WorkItemHistoryController> logger, IWorkItemService workItemService)
        {
            _workItemHistoryService = workItemHistoryService;
            _logger = logger;
            _workItemService = workItemService;
        }


        [HttpGet("get/{workItemID}")]
        [Authorize]
        public async Task<IActionResult> GetWorkItemHistoryForWorkItem(Guid workItemID)
        {
            try
            {
                
                var workItem =  await _workItemService.GetByIdAsync(workItemID);
                if (workItem == null)
                {
                     return NotFound("work item not found with this id ");
                }
              
                var workItemHistorys= await _workItemHistoryService.GetByWorkItemHistorysByWorkItemIdAsync(workItemID);
                return Ok(workItemHistorys);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return StatusCode(500,ex.Message);
            }


        }


    }
}
