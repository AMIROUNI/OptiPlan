using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using OptiPlanBackend.Models;
using OptiPlanBackend.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace OptiPlanBackend.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IDirectMessageService _messageService;
        private readonly IDirectChatService _chatService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserService _userService;
        private readonly ILogger<ChatHub> _logger;

        public ChatHub(
            IDirectMessageService messageService,
            IDirectChatService chatService,
            ICurrentUserService currentUserService,
            IUserService userService,
            ILogger<ChatHub> logger) // Add ILogger dependency
        {
            _messageService = messageService;
            _chatService = chatService;
            _currentUserService = currentUserService;
            _userService = userService;
            _logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            _logger.LogInformation("Client connected: {ConnectionId}, User: {UserIdentifier}",
                Context.ConnectionId, Context.UserIdentifier);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (exception != null)
            {
                _logger.LogWarning(exception, "Client disconnected with error: {ConnectionId}",
                    Context.ConnectionId);
            }
            else
            {
                _logger.LogInformation("Client disconnected: {ConnectionId}", Context.ConnectionId);
            }
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string receiverUsername, string content)
        {
            try
            {
                _logger.LogInformation("SendMessage called by {UserIdentifier} to {ReceiverUsername}",
                    Context.UserIdentifier, receiverUsername);

                var senderId = _currentUserService.UserId.Value;
                if (senderId == null)
                {
                    _logger.LogWarning("User not authenticated for SendMessage");
                    throw new HubException("User not authenticated");
                }

                var user = await _userService.GetUserByUsernameAsync(receiverUsername);
                if (user == null)
                {
                    _logger.LogWarning("User {ReceiverUsername} not found", receiverUsername);
                    throw new HubException($"User {receiverUsername} not found");
                }

                // Check if chat exists between users
                var chat = await _chatService.GetPrivateChatAsync(senderId, user.Id);
                if (chat == null)
                {
                    _logger.LogInformation("Creating new chat between {SenderId} and {ReceiverId}",
                        senderId, user.Id);
                    chat = await _chatService.CreatePrivateChatAsync(senderId, user.Id);
                }

                // Create message
                var message = new DirectMessage
                {
                    DirectChatId = chat.Id,
                    SenderId = senderId,
                    Content = content
                };

                await _messageService.CreateAsync(message);

                _logger.LogInformation("Sending message to group: {ChatId}, Content: {Content}",
                    chat.Id, content);

                await Clients.Group(chat.Id.ToString()).SendAsync("ReceiveMessage", new
                {
                    senderId,
                    content,
                    displaySender = user.Username,
                    sentAt = message.SentAt,
                    chatId = chat.Id
                });

                _logger.LogInformation("Message sent successfully to chat {ChatId}", chat.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SendMessage from {UserIdentifier} to {ReceiverUsername}",
                    Context.UserIdentifier, receiverUsername);
                throw new HubException($"Failed to send message: {ex.Message}");
            }
        }

        public async Task JoinChat(string receiverUsername)
        {
            try
            {
                _logger.LogInformation("JoinChat called for user: {ReceiverUsername} by {UserIdentifier}",
                    receiverUsername, Context.UserIdentifier);

                var senderId = _currentUserService.UserId.Value;
                if (senderId == null)
                {
                    _logger.LogWarning("User not authenticated for JoinChat");
                    throw new HubException("User not authenticated");
                }

                var user = await _userService.GetUserByUsernameAsync(receiverUsername);
                if (user == null)
                {
                    _logger.LogWarning("User {ReceiverUsername} not found for JoinChat", receiverUsername);
                    throw new HubException($"User {receiverUsername} not found");
                }

                var chat = await _chatService.GetPrivateChatAsync(senderId, user.Id);
                if (chat != null)
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, chat.Id.ToString());
                    _logger.LogInformation("User {UserIdentifier} added to group: {ChatId}",
                        Context.UserIdentifier, chat.Id);
                }
                else
                {
                    _logger.LogWarning("No chat found between {SenderId} and {ReceiverId}",
                        senderId, user.Id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in JoinChat for user: {ReceiverUsername}", receiverUsername);
                throw new HubException($"Failed to join chat: {ex.Message}");
            }
        }

        public async Task LeaveChat(string receiverUsername)
        {
            try
            {
                _logger.LogInformation("LeaveChat called for user: {ReceiverUsername} by {UserIdentifier}",
                    receiverUsername, Context.UserIdentifier);

                var senderId = _currentUserService.UserId.Value;
                var user = await _userService.GetUserByUsernameAsync(receiverUsername);

                if (user != null)
                {
                    var chat = await _chatService.GetPrivateChatAsync(senderId, user.Id);
                    if (chat != null)
                    {
                        await Groups.RemoveFromGroupAsync(Context.ConnectionId, chat.Id.ToString());
                        _logger.LogInformation("User {UserIdentifier} removed from group: {ChatId}",
                            Context.UserIdentifier, chat.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in LeaveChat for user: {ReceiverUsername}", receiverUsername);
            }
        }
    }
}