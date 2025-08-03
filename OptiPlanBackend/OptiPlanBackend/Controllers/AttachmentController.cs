using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OptiPlanBackend.Data;
using OptiPlanBackend.Dto;
using OptiPlanBackend.Models;
using OptiPlanBackend.Services.Implementations;
using OptiPlanBackend.Services.Interfaces;

namespace OptiPlanBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttachmentController : ControllerBase
    {

        private readonly ICurrentUserService _currentUserService;
        private readonly IAttachmentService _attachmentService;
        private readonly IWorkItemService _workItemService;
        private readonly ILogger<AttachmentController> _logger;

        private readonly IUserService _userService;

        private readonly IUploadService _uploadService;

        public AttachmentController(
            ICurrentUserService currentUserService,
            IAttachmentService attachmentService,
            IWorkItemService workItemService,
            ILogger<AttachmentController> logger,
            IUploadService uploadService,
            IUserService userService) 
        {
            _currentUserService = currentUserService;
            _attachmentService = attachmentService;
            _logger = logger;
            _workItemService = workItemService;
            _uploadService = uploadService;
            _userService = userService;
        }



        [HttpGet("get/{workItemId}")]
        [Authorize]
        public async Task<IActionResult> GetAllAttachmentsForWorkItem([FromRoute] Guid workItemId)
        {
            try
            {
                _logger.LogWarning($"Fetching attachments for WorkItemId: {workItemId}");

                var attachement = await _attachmentService.GetByAttachmentsByWorkItemIdAsync(workItemId);

                return Ok(attachement);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting attachments");
                return StatusCode(500, ex.Message);
            }
        }




        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> Create([FromForm] AttachmentDto attachmentDto)
        {
            try
            {
                var userId = _currentUserService.UserId;
                if (!userId.HasValue)
                    return Unauthorized();
                _logger.LogWarning("***************************** {userId}", userId);
                var userExists = await  _userService.GetUserByIdAsync(userId.Value);
                if (userExists == null)
                    return BadRequest("Uploader user does not exist in the database.");

                var attachment = new Attachment
                {
                    Id = Guid.NewGuid(),
                    FileName = attachmentDto.File.FileName,
                    UploadedAt = DateTime.UtcNow,
                    WorkItemId = attachmentDto.WorkItemId,
                    UploaderId = userId.Value
                };

                var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "Attachments");

                if (!Directory.Exists(uploadDir))
                    Directory.CreateDirectory(uploadDir);

                var filePath = Path.Combine(uploadDir, attachment.FileName);

              
                if (System.IO.File.Exists(filePath))
                {
                    _logger.LogWarning("File already exists: " + filePath);

                    // Vérifie s’il est déjà en base
                    var existing = await  _attachmentService.GetAttachmentByWorkItemIdAndFileNameAndUploaderId(attachment.WorkItemId,
                        attachment.FileName,attachment.UploaderId);

                    if (existing != null)
                    {
                        return Ok(existing); 
                    }

                   // return Conflict("A file with this name already exists but is not linked in DB.");
                }

                //  Sauvegarde physique
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await attachmentDto.File.CopyToAsync(stream);
                }

                attachment.FilePath = Path.Combine("Attachments", attachment.FileName); // relative

                _attachmentService.CreateAsync(attachment);
                _logger.LogWarning(attachment.ToString());

                return Ok(attachment);
            }
            catch (Exception ex)
            {
                _logger.LogError("Attachment upload failed: " + ex.Message);
                return StatusCode(500, "Failed to upload attachment: " + ex.Message);
            }
        }




    }
}
