using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OptiPlanBackend.Services.Interfaces;

namespace OptiPlanBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly ILogger<CommentController> _logger;
        private readonly ICurrentUserService _currentUserService;
        public CommentController(ICommentService commentService, ILogger<CommentController> logger, ICurrentUserService currentUserService)
        {
            _commentService = commentService;
            _logger = logger;
            _currentUserService = currentUserService;
        }



        [HttpGet("get/{workIteamId}")]
        public async Task<IActionResult> GetAllByWorkIteamId(Guid workIteamId)
        {
            try
            { 

                var comments = await _commentService.GetCommentsByWorkItemIdAsync(workIteamId);
                return Ok(comments);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching comments");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error fetching comments");
            }
        }


        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] Models.Comment comment)
        {
            if (comment == null)
            {
                return BadRequest("Comment cannot be null.");
            }
            try
            {
                comment.AuthorId = _currentUserService.UserId.Value;
                var result = await _commentService.CreateAsync(comment);
                if (result)
                {
                    return CreatedAtAction(nameof(GetAllByWorkIteamId), new { workIteamId = comment.WorkItemId }, comment);
                }
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating comment");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating comment");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating comment");
            }

        }



        }
}
