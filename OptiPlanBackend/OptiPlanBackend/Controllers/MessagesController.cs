using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OptiPlanBackend.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace OptiPlanBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MessagesController : ControllerBase
    {
        private readonly IDirectMessageService _messageService;
        private readonly IDirectChatService _chatService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserService _userService;
        private readonly ILogger<MessagesController> _logger;

        public MessagesController(
            IDirectMessageService messageService,
            IDirectChatService chatService,
            ICurrentUserService currentUserService,
            IUserService userService,
            ILogger<MessagesController> logger)
        {
            _messageService = messageService;
            _chatService = chatService;
            _currentUserService = currentUserService;
            _userService = userService;
            _logger = logger;
        }

        [HttpGet("history/{receiverUsername}")]
        public async Task<IActionResult> GetMessageHistory(string receiverUsername)
        {
            try
            {
                _logger.LogInformation("Getting message history for user: {ReceiverUsername}", receiverUsername);

                var currentUserId = _currentUserService.UserId.Value;
                if (currentUserId == null)
                {
                    return Unauthorized();
                }

                // Get receiver user
                var receiverUser = await _userService.GetUserByUsernameAsync(receiverUsername);
                if (receiverUser == null)
                {
                    return NotFound($"User {receiverUsername} not found");
                }

                // Get or create chat
                var chat = await _chatService.GetPrivateChatAsync(currentUserId, receiverUser.Id);
                if (chat == null)
                {
                    // No chat exists yet, return empty list
                    return Ok(new object[] { });
                }

                // Get all messages for this chat
                var messages = await _messageService.GetMessagesByChatIdAsync(chat.Id);

              

                return Ok(messages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting message history for {ReceiverUsername}", receiverUsername);
                return StatusCode(500, new { error = ex.Message });
            }
        }

        private async Task<string> GetUsernameAsync(Guid userId)     
        {
            var user = await _userService.GetUserByIdAsync(userId);
            return user?.Username ?? userId.ToString();
        }


        [HttpGet("uesrs-i-have-chat")]
        [Authorize]
         public async Task<IActionResult> GetUsersIHaveChatWithIt()
        {
            try
            {
                var userId = _currentUserService.UserId.Value;
                var users = await _chatService.GetUsersIHaveChatWithIt(userId);

                return Ok(users);
            }catch(Exception ex){

                _logger.LogError(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

    }
}