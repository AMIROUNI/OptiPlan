using Microsoft.EntityFrameworkCore;
using OptiPlanBackend.Data;
using OptiPlanBackend.Enums;
using OptiPlanBackend.Models;
using OptiPlanBackend.Repositories.Interfaces;

namespace OptiPlanBackend.Repositories.Implementations
{
    public class ProjectRepository : GenericRepository<Project>, IProjectRepository
    {

        private readonly  UserDbContext _context;
        public ProjectRepository(UserDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Project>> GetByOwnerIdAsync(Guid ownerId)
        {
            return await FindAsync(p => p.OwnerId == ownerId);
        }


        public async Task<IEnumerable<Project>> GetProjectsForUserAsync(Guid userId)
        {
            return await _context.Projects
                .Include(p => p.Owner)
                .Include(p => p.Team)
                    .ThenInclude(t => t.Members)
                .Include(p => p.Team)
                    .ThenInclude(t => t.Invitations)
                .Where(p =>
                    p.OwnerId == userId ||  // User is the owner
                    p.Team.Members.Any(m => m.UserId == userId) ||  // User is a team member
                    p.Team.Invitations.Any(i =>
                        i.InviteeId == userId &&
                        i.Status == InvitationStatus.Accepted)  // User has accepted invitation
                )
                .AsNoTracking()
                .ToListAsync();
        }

    }
}
