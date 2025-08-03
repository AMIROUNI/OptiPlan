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
            var allProjects = await _context.Projects
                .Include(p => p.Owner)
                .Include(p => p.Team)
                    .ThenInclude(t => t.Members)
                .Include(p => p.Team)
                    .ThenInclude(t => t.Invitations)
                .AsNoTracking()
                .ToListAsync();

            return allProjects.Where(p =>
                p.OwnerId == userId ||
                (p.Team != null &&
                    (
                        p.Team.Members.Any(m => m.UserId == userId) ||
                        p.Team.Invitations.Any(i =>
                            i.InviteeId == userId &&
                            i.Status == InvitationStatus.Accepted
                        )
                    )
                )
            );
        }


        public async Task<Team> GetTeamByProjectId(Guid projectId)
        {
            return await _context.Teams
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.Project.Id == projectId);
        }
        public async Task<IEnumerable<User>> GetUsersByProjectIdAsync(Guid projectId)
        {
            var team = await _context.Teams
                .Include(t => t.Members)
                    .ThenInclude(m => m.User)
                .FirstOrDefaultAsync(t => t.ProjectId == projectId);

            if (team == null)
                return Enumerable.Empty<User>();

            // On retourne uniquement les Users
            return team.Members
                       .Where(m => m.Status == MembershipStatus.Accepted) // facultatif : filtrer les membres actifs
                       .Select(m => m.User);
        }



    }
}
