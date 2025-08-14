using Microsoft.EntityFrameworkCore;
using OptiPlanBackend.Data;
using OptiPlanBackend.Models;
using OptiPlanBackend.Repositories.Interfaces;

namespace OptiPlanBackend.Repositories.Implementations
{
    public class DirectChatRepository : GenericRepository<DirectChat>, IDirectChatRepository
    {
         private readonly UserDbContext _context;
        public DirectChatRepository(UserDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<DirectChat?> GetPrivateChatAsync(Guid user1Id, Guid user2Id)
        {
            return await _context.DirectChats
                .Include(dc => dc.Messages)
                .FirstOrDefaultAsync(dc =>
                    (dc.User1Id == user1Id && dc.User2Id == user2Id) ||
                    (dc.User1Id == user2Id && dc.User2Id == user1Id));
        }

        public async Task<DirectChat> CreatePrivateChatAsync(Guid user1Id, Guid user2Id)
        {
            var chat = new DirectChat
            {
                User1Id = user1Id,
                User2Id = user2Id
            };

            _context.DirectChats.Add(chat);
            await _context.SaveChangesAsync();
            return chat;
        }
    }
}
