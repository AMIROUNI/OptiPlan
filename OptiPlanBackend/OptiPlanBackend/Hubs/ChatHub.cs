using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using OptiPlanBackend.Models;
using OptiPlanBackend.Services.Interfaces;

namespace OptiPlanBackend.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IDirectMessageService _messageService;
        private readonly IDirectChatService _chatService;
        private readonly ICurrentUserService _currentUserService;

        public ChatHub(
            IDirectMessageService messageService,
            IDirectChatService chatService,
            ICurrentUserService currentUserService)
        {
            _messageService = messageService;
            _chatService = chatService;
            _currentUserService = currentUserService;
        }

        public async Task SendMessage(Guid receiverId, string content)
        {
            var senderId = _currentUserService.UserId.Value;

            //  Vérifie si le chat entre les deux utilisateurs existe
            var chat = await _chatService.GetPrivateChatAsync(senderId, receiverId);

            //  S'il n'existe pas, on le crée
            if (chat == null)
            {
                chat = await _chatService.CreatePrivateChatAsync(senderId, receiverId);
            }

            //  Crée le message
            var message = new DirectMessage
            {
                DirectChatId = chat.Id,
                SenderId = senderId,
                Content = content
            };

            await _messageService.CreateAsync(message);

         
            await Clients.Group(chat.Id.ToString()).SendAsync("ReceiveMessage", new
            {
                senderId,
                content,
                sentAt = message.SentAt,
                chatId = chat.Id
            });
        }


        

        public async Task LeaveChat(Guid chatId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId.ToString());
        }

        public async Task JoinChat(Guid receiverId)
        {
            var senderId = _currentUserService.UserId.Value;

            var chat = await _chatService.GetPrivateChatAsync(senderId, receiverId);
            if (chat != null)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, chat.Id.ToString());
            }
        }

    }

}
