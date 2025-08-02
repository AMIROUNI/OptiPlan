using Microsoft.EntityFrameworkCore;
using OptiPlanBackend.Data;
using OptiPlanBackend.Enums;
using OptiPlanBackend.Models;
using OptiPlanBackend.Repositories.Interfaces;

namespace OptiPlanBackend.Repositories.Implementations
{
    public class TeamRepository : GenericRepository<Team>, ITeamRepository
    {

        private readonly UserDbContext _context;
        public TeamRepository(UserDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<TeamRole?> GetUserRoleInProjectAsync(Guid userId, Guid projectId)
        {
            var teamMembership = await _context.TeamMemberships
            .Include(tm => tm.Team)
            .FirstOrDefaultAsync(tm => tm.UserId == userId && tm.Team.ProjectId == projectId);

            return teamMembership?.Role;
        }
    }
}
