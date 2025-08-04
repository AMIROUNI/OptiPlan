using Microsoft.EntityFrameworkCore;
using OptiPlanBackend.Data;
using OptiPlanBackend.Models;
using OptiPlanBackend.Repositories.Interfaces;

namespace OptiPlanBackend.Repositories.Implementations
{
    public class ConversationRepository : GenericRepository<Conversation>, IConversationRepository
    {
        private readonly UserDbContext _context;
        public ConversationRepository(UserDbContext context) : base(context)
        {
            _context = context;
        }



        public async Task<Conversation?> GetConversationByIdIncludeMessages(Guid conversationId)
        {
            return await _context.Conversations
                .Include(c => c.Messages)   // Charger les messages liés
                .FirstOrDefaultAsync(c => c.Id == conversationId);
        }

        public async Task<IEnumerable<Conversation> > GetConversationByUserId(Guid userId)
        {
            return await _context.Conversations.Where(c => c.UserId == userId)
                   .ToListAsync();
        }
    }
}
