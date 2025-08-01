using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OptiPlanBackend.Data;
using OptiPlanBackend.Dto;
using OptiPlanBackend.Enums;
using OptiPlanBackend.Models;
using OptiPlanBackend.Services.Implementations;
using OptiPlanBackend.Services.Interfaces;

namespace OptiPlanBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvitationController : ControllerBase
    {

        private readonly IInvitationService _invitationService;
        private readonly IEmailService _emailService;
        private readonly ILogger<InvitationController> _logger;
        private readonly UserDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public InvitationController(IInvitationService invitationService
        , IEmailService emailService,
          ILogger<InvitationController> logger,
         UserDbContext context,
         ICurrentUserService currentUserService)
        {
            _invitationService = invitationService;
            _emailService = emailService;
            _logger = logger;
            _context = context;
            _currentUserService = currentUserService;
        }


        [HttpPost("send")]
        [Authorize]
        public async Task<IActionResult> Send([FromBody] InvitationDto request)
        {
            try
            {
                
                var inviter = await _context.Users.FindAsync(request.InviterId);
                if (inviter == null)
                    return BadRequest(new { error = "Invalid InviterId", message = $"User with ID {request.InviterId} does not exist." });

                var invitee = await _context.Users.FindAsync(request.InviteeId);
                if (invitee == null)
                    return BadRequest(new { error = "Invalid InviteeId", message = $"User with ID {request.InviteeId} does not exist." });

                var team = await _context.Teams
                    .Include(t => t.Project) 
                    .FirstOrDefaultAsync(t => t.Id == request.TeamId);
                if (team == null)
                    return BadRequest(new { error = "Invalid TeamId", message = $"Team with ID {request.TeamId} does not exist." });

                // Générer un ID unique pour le lien d'invitation
                Guid invitationId = Guid.NewGuid();

                string subject = "You're Invited to OptiPlan!";
                string htmlBody = GenerateHtmlEmail(
                    request.Email,
                    team.Project?.Title ?? "OptiPlan Project",
                    team.Name,
                    inviter.Username,
                    invitationId
                );

                await _emailService.SendEmailAsync(request.Email, subject, htmlBody);

                var invitation = new Invitation
                {
                    Id = invitationId,
                    Email = request.Email,
                    TeamId = request.TeamId,
                    InviteeId = request.InviteeId,
                    InviterId = request.InviterId,
                    Role = request.Role,
                    SentAt = DateTime.UtcNow
                };

                await _invitationService.CreateAsync(invitation);

                return Ok(new { message = "Invitation sent and saved successfully." });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { error = "Database Error", message = ex.InnerException?.Message ?? ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Server Error", message = ex.Message });
            }


           
        }




        [HttpPost("accept/{InvitationId}")]
        [Authorize]
        public async Task<IActionResult> AcceptInvitation(Guid InvitationId)
        {
            try
            {
                var invitation = await _invitationService.GetByIdAsync(InvitationId);
                if (invitation == null)
                    return NotFound(new { error = "Invitation not found" });
                if (invitation.Status != InvitationStatus.Pending)
                    return BadRequest(new { error = "Invitation already responded to" });
                invitation.Status = InvitationStatus.Accepted;
                invitation.RespondedAt = DateTime.UtcNow;
                invitation.InviteeId = _currentUserService.UserId;
                invitation.IsAccepted = true;
                await _invitationService.UpdateAsync(invitation);
                // Add the user to the team
                var teamMembership = new TeamMembership
                {
                    UserId = (Guid)_currentUserService.UserId,
                    TeamId = invitation.TeamId,
                    Role = invitation.Role
                };
                await _context.TeamMemberships.AddAsync(teamMembership);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Invitation accepted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error accepting invitation");
                return StatusCode(500, new { error = "Server Error", message = ex.Message });
            }
        }



        [HttpGet("get")]
        [Authorize]
        public async Task<IActionResult> GetInvitations()
        {
            try
            {
                var userId = _currentUserService.UserId;
                if (userId == null)
                    return Unauthorized(new { error = "User not authenticated" });
                var invitations = await _invitationService.GetByUserIdAsync((Guid)userId);
                if (invitations == null || !invitations.Any())
                    return NotFound(new { message = "No invitations found" });
                return Ok(invitations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving invitations");
                return StatusCode(500, new { error = "Server Error", message = ex.Message });
            }


            
        }


        [HttpPost("reject/{InvitationId}")]
        [Authorize]
        public async Task<IActionResult> Reject(Guid InvitationId)
        {
            try
            {
                var invitation = await _invitationService.GetByIdAsync(InvitationId);
                if (invitation == null)
                    return NotFound(new { error = "Invitation not found" });

                if (invitation.Status != InvitationStatus.Pending)
                    return BadRequest(new { error = "Invitation already responded to" });

                invitation.Status = InvitationStatus.Rejected;
                invitation.RespondedAt = DateTime.UtcNow;
                invitation.InviteeId = _currentUserService.UserId;
                invitation.IsAccepted = false;

                bool updated = await _invitationService.UpdateAsync(invitation);

                if (!updated)
                    return StatusCode(500, new { error = "Failed to update the invitation" });

                return Ok(new { message = "Invitation rejected successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rejecting invitation");
                return StatusCode(500, new { error = "Server Error", message = ex.Message });
            }
        }





































        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private string GenerateHtmlEmail(string recipientEmail, string projectTitle, string teamName, string inviterName, Guid invitationId)
        {
            string joinLink = $"https://optiplan.com/invite/{invitationId}"; // You can make it dynamic

            return $@"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <title>OptiPlan Invitation</title>
    <style>
        body {{
            font-family: 'Segoe UI', sans-serif;
            background-color: #f4f4f4;
            margin: 0;
            padding: 0;
        }}
        .container {{
            max-width: 600px;
            background-color: #fff;
            margin: 30px auto;
            padding: 25px;
            border-radius: 10px;
            box-shadow: 0 2px 8px rgba(0,0,0,0.1);
        }}
        h1 {{
            color: #1f2937;
        }}
        p {{
            color: #4b5563;
            font-size: 16px;
        }}
        .button {{
            display: inline-block;
            background-color: #3b82f6;
            color: white;
            padding: 12px 24px;
            border-radius: 6px;
            text-decoration: none;
            font-weight: bold;
            margin-top: 20px;
        }}
        .footer {{
            margin-top: 40px;
            font-size: 12px;
            color: #9ca3af;
            text-align: center;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <h1>You're invited to OptiPlan</h1>
        <p>Hi there 👋,</p>
        <p><strong>{inviterName}</strong> has invited you to join the team <strong>{teamName}</strong> for the project <strong>{projectTitle}</strong> on <strong>OptiPlan</strong>.</p>
        <p>Click below to accept the invitation and start collaborating:</p>
        <a href='{joinLink}' class='button'>Join Now</a>
        <div class='footer'>
            © 2025 OptiPlan. All rights reserved.
        </div>
    </div>
</body>
</html>";
        }




    }

}

