using Microsoft.VisualBasic;
using OptiPlanBackend.Data;
using OptiPlanBackend.Models;
using OptiPlanBackend.Repositories.Interfaces;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;


namespace OptiPlanBackend.Repositories.Implementations
{
    public class TeamMembershipRepository : GenericRepository<TeamMembership>, ITeamMembershipRepository
    {

        private readonly UserDbContext _context;
        public TeamMembershipRepository(UserDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TeamMembership>> GetByProjectIdAsync(Guid projectId)
        {
             return  await _context.TeamMemberships
                .Where(tm=> tm.Team.ProjectId == projectId)
                .Include(tm => tm.User) 
                .ToListAsync();
        }

        public async Task<TeamMembership?> GetByUserAndProjectIdAsync(Guid userId, Guid projectId)
        {
           return await _context.TeamMemberships
                .Include(tm => tm.User)
                .FirstOrDefaultAsync(tm => tm.UserId == userId && tm.Team.ProjectId == projectId);
        }

        public  async Task<IEnumerable<TeamMembership>> GetByUserIdAsync(Guid userId)
        {
             return await _context.TeamMemberships
                .Where(tm => tm.UserId == userId)
                .Include(tm => tm.User)
                .Include(tm => tm.Team)
                .ToListAsync();
        }
    }
}
