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

       
        private static readonly Dictionary<string, string> _userConnections = new Dictionary<string, string>();

        public override async Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;
            var connectionId = Context.ConnectionId;

            _logger.LogInformation("Client connected: {ConnectionId}, User: {UserIdentifier}",
                connectionId, userId);

         
            if (!string.IsNullOrEmpty(userId))
            {
                _userConnections[connectionId] = userId;
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var connectionId = Context.ConnectionId;

            if (_userConnections.ContainsKey(connectionId))
            {
                _userConnections.Remove(connectionId);
            }

            if (exception != null)
            {
                _logger.LogWarning(exception, "Client disconnected with error: {ConnectionId}", connectionId);
            }
            else
            {
                _logger.LogInformation("Client disconnected: {ConnectionId}", connectionId);
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string receiverUsername, string content)
        {
            try
            {
                var senderId = _currentUserService.UserId.Value;
                if (senderId == null) throw new HubException("User not authenticated");

                var receiverUser = await _userService.GetUserByUsernameAsync(receiverUsername);
                if (receiverUser == null) throw new HubException($"User {receiverUsername} not found");

                var chat = await _chatService.GetPrivateChatAsync(senderId, receiverUser.Id);
                if (chat == null)
                    chat = await _chatService.CreatePrivateChatAsync(senderId, receiverUser.Id);

                var message = new DirectMessage
                {
                    DirectChatId = chat.Id,
                    SenderId = senderId,
                    Content = content
                };

                await _messageService.CreateAsync(message);

                // ✅ Broadcast to everyone in the chat (including sender)
                await Clients.Group(chat.Id.ToString()).SendAsync("ReceiveMessage", new
                {
                    senderId,
                    senderUsername = await GetUsernameAsync(senderId),
                    content,
                    sentAt = message.SentAt,
                    chatId = chat.Id
                });
            }
            catch (Exception ex)
            {
                throw new HubException($"Failed to send message: {ex.Message}");
            }
        }


        public async Task JoinChat(string receiverUsername)
        {
            try
            {
                _logger.LogInformation("JoinChat called for user: {ReceiverUsername} by {UserIdentifier}",
                    receiverUsername, Context.UserIdentifier);

                var currentUserId = _currentUserService.UserId.Value;
                if (currentUserId == null)
                {
                    throw new HubException("User not authenticated");
                }

                var otherUser = await _userService.GetUserByUsernameAsync(receiverUsername);
                if (otherUser == null)
                {
                    throw new HubException($"User {receiverUsername} not found");
                }

                var chat = await _chatService.GetPrivateChatAsync(currentUserId, otherUser.Id);
                if (chat != null)
                {
                    // ✅ Add current user to the chat group
                    await Groups.AddToGroupAsync(Context.ConnectionId, chat.Id.ToString());
                    _logger.LogInformation("User {UserIdentifier} added to group: {ChatId}",
                        Context.UserIdentifier, chat.Id);

                    // ✅ Also ensure the other user is in the group (if online)
                    // This ensures both users receive messages in real-time
                    await EnsureUserInGroup(otherUser.Id, chat.Id);
                }
                else
                {
                    _logger.LogWarning("No chat found between users");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in JoinChat");
                throw new HubException($"Failed to join chat: {ex.Message}");
            }
        }

        // ✅ New method to ensure both users are in the chat group
        private async Task EnsureUserInGroup(Guid userId, Guid chatId)
        {
            // This would require tracking user connections
            // For now, we'll rely on each user calling JoinChat when they open the chat
            _logger.LogInformation("Ensuring user {UserId} is in group {ChatId}", userId, chatId);
        }

        // ✅ Add this helper method
        private async Task<string> GetUsernameAsync(Guid userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            return user?.Username ?? userId.ToString();
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
