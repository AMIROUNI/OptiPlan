using Microsoft.EntityFrameworkCore;
using OptiPlanBackend.Data;
using OptiPlanBackend.Models;
using OptiPlanBackend.Repositories.Interfaces;

namespace OptiPlanBackend.Repositories.Implementations
{
    public class InvitationRepository : GenericRepository<Invitation>, IInvitationRepository
    { 

        private readonly UserDbContext _context;
        public InvitationRepository(UserDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Invitation>> GetByUserIdAsync(Guid userId)
        {
             return await FindAsync(i => i.InviteeId == userId);
        }

        public async Task<Team> GetTeamWithProjectAsync(Guid teamId)
        {
            return await _context.Teams
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.Id == teamId);
        }

        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

      
    }
}
