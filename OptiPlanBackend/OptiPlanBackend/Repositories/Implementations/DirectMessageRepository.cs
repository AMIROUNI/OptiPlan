using Microsoft.EntityFrameworkCore;
using OptiPlanBackend.Data;
using OptiPlanBackend.Models;
using OptiPlanBackend.Repositories.Interfaces;

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
    }

     
    }
