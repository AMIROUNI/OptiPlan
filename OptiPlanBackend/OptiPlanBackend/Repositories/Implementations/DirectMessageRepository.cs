using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using OptiPlanBackend.Data;
using OptiPlanBackend.Dto;
using OptiPlanBackend.Models;
using OptiPlanBackend.Repositories.Interfaces;
using OptiPlanBackend.Services.Interfaces;

namespace OptiPlanBackend.Repositories.Implementations
{
    public class DirectMessageRepository : GenericRepository<DirectMessage>, IDirectMessageRepository
    {
        private readonly UserDbContext _context;

        public DirectMessageRepository(UserDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<DirectMessage>> GetMessagesAsync(Guid chatId)
        {
            return await _context.DirectMessages
                .Where(m => m.DirectChatId == chatId)
                .Include(m => m.Sender)
                .OrderBy(m => m.SentAt)
                .ToListAsync();
        }


        public async Task<List<MessageDto>> GetMessagesByChatIdAsync(Guid chatId)
        {
            // Get all messages first (materialize query)
            var messages = await _context.DirectMessages
                .Where(m => m.DirectChatId == chatId)
                .ToListAsync();

            var messageDtos = new List<MessageDto>();

            // Sequentially fetch sender info for each message
            foreach (var msg in messages)
            {
                var user = await _context.Users
                    .AsNoTracking() // optional: improves performance
                    .FirstOrDefaultAsync(u => u.Id == msg.SenderId);

                messageDtos.Add(new MessageDto
                {
                    Id = msg.Id,
                    DirectChatId = msg.DirectChatId,
                    SenderId = msg.SenderId,
                    SenderUsername = user?.Username ?? "Unknown",
                    DisplaySender= user?.Username ?? "Unknown",
                    Content = msg.Content,
                    SentAt = msg.SentAt
                });
            }

            return messageDtos;
        }
    }


    }
                   