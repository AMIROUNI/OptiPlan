using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OptiPlanBackend.Dto;
using OptiPlanBackend.Models;
using OptiPlanBackend.Services;
using OptiPlanBackend.Services.Interfaces;

namespace OptiPlanBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatbotController : ControllerBase
    {
        private readonly IChatBotService _chatService;
        private readonly ILogger<ChatbotController> _logger;
        private readonly IChatMessageService _chatMessageService;
        private readonly IConversationService _conversationService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserService _userService;

        public ChatbotController(IChatBotService chatService
            , ILogger<ChatbotController> logger,
            IChatMessageService chatMessageService, 
            IConversationService conversationService,
            ICurrentUserService currentUserService,
            IUserService userService)
        {
            _chatService = chatService;
            _logger = logger;
            _chatMessageService = chatMessageService;
            _conversationService = conversationService;
            _currentUserService = currentUserService;
            _userService = userService;
        }

        [HttpPost("ask")]
        [Authorize]
        public async Task<IActionResult> AskBot([FromBody] ChatRequestDto request)
        {
            try
            {
                Guid conversationId;

                // Si ConversationId n'est pas fourni créer une nouvelle conversation
                if (request.ConversationId == null)
                {
                    var newConversation = new Conversation
                    {
                        Id = Guid.NewGuid(),
                        Title = request.Message.Length > 15 ? request.Message.Substring(0, 15) : request.Message,
                        CreatedAt = DateTime.UtcNow,
                       
                        UserId = _currentUserService.UserId.Value,
                    };

                    await _conversationService.CreateAsync(newConversation);
                    conversationId = newConversation.Id;
                }
                else
                {
                    conversationId = request.ConversationId.Value;
                }

                // Sauvegarder le message utilisateur
                var userMessage = new ChatMessage
                {
                    Id = Guid.NewGuid(),
                    Content = request.Message,
                    Role = "user",
                    SentAt = DateTime.UtcNow,
                    ConversationId = conversationId
                };
                await _chatMessageService.CreateAsync(userMessage);

                // Obtenir la réponse du bot
                var reply = await _chatService.AskBotAsync(request.Message);

                // Sauvegarder la réponse du bot
                var botMessage = new ChatMessage
                {
                    Id = Guid.NewGuid(),
                    Content = reply,
                    Role = "assistant",
                    SentAt = DateTime.UtcNow,
                    ConversationId = conversationId
                };
                await _chatMessageService.CreateAsync(botMessage);

                return Ok(new
                {
                    response = reply,
                    conversationId = conversationId
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("get-conversation/{conversationId}")]
        public async Task<IActionResult> GetConversationIncludMessages(Guid conversationId)
        {
            try
            {

                var conversation = await _conversationService.GetConversationByIdIncludeMessages(conversationId);
                if (conversation == null)
                    NotFound("no converstaion with this id ");



                return Ok(conversation);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, ex.Message);
            }

        }



        [HttpDelete("delete-conversation/{conversationId}")]

        public async Task<IActionResult> DeleteConversation(Guid conversationId)
        {
            try
            {

                var conversation = await _conversationService.GetByIdAsync(conversationId);
                if (conversation == null)
                    return NotFound("no conversation with this id ");


                await _conversationService.DeleteAsync(conversation);

                return Ok(true);


            }
            catch(Exception ex)
            {

                _logger.LogError(ex.Message);
                return StatusCode(500, ex.Message);

            }
        }



        [HttpGet("get-conversations/user/{userId}")]
        public async Task<IActionResult> GetConversationsByUserId(Guid userId)
        {
            try
            {
                var user =  await
                     _userService.GetUserByIdAsync(userId);
                if (user == null)
                    return NotFound("user does not exsite ");




                var conversations = await _conversationService.GetConversationByUserId(userId);
                if (conversations == null)
                    NotFound("no converstaion for  this user  id ");



                return Ok(conversations);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, ex.Message);
            }

        }



        [HttpPost("new-conversation")]
        [Authorize]
        public async Task<IActionResult> NewConversation([FromBody] NewConversationDto request)
        {
            try
            {
                var conversation = new Conversation
                {
                    Id = Guid.NewGuid(),
                    Title = string.IsNullOrWhiteSpace(request.Title) ? "Default Conversation" : request.Title,
                    CreatedAt = DateTime.UtcNow,
                    UserId = _currentUserService.UserId.Value
                };

                await _conversationService.CreateAsync(conversation);

                return Ok(new { conversationId = conversation.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

    }




}


