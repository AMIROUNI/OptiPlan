using OptiPlanBackend.Data;
using OptiPlanBackend.Models;
using OptiPlanBackend.Repositories.Interfaces;

namespace OptiPlanBackend.Repositories.Implementations
{
    public class ChatMessageRepository : GenericRepository<ChatMessage>, IChatMessageRepository
    {
        private readonly UserDbContext _context;
        public ChatMessageRepository(UserDbContext context) : base(context)
        {
            _context = context;
        }


    }
}
