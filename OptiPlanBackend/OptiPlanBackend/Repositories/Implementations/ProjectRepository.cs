using Microsoft.EntityFrameworkCore;
using OptiPlanBackend.Data;
using OptiPlanBackend.Dto;
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

        public async Task<IEnumerable<SimpleUserDto>> GetAllUsersOfProjectAsync(Guid projectId)
        {
            Console.WriteLine($"[DEBUG] Searching users for projectId = {projectId}");

            var memberships = await _context.TeamMemberships
                .Include(m => m.User)
                .Include(m => m.Team)
                .Where(m => m.Team.ProjectId == projectId)
                .ToListAsync();

            Console.WriteLine($"[DEBUG] Total TeamMemberships for project = {memberships.Count}");

            foreach (var m in memberships)
            {
                Console.WriteLine($"[DEBUG] Membership: UserId={m.UserId}, Status={m.Status}, HasUser={(m.User != null)}");
            }

            var acceptedMembers = memberships
                .Where(m => m.Status == MembershipStatus.Accepted && m.User != null)
                .Select(m => new SimpleUserDto
                {
                    Id = m.User.Id,
                    Username = m.User.Username,
                    FullName = m.User.FullName,
                    Email = m.User.Email,
                    AvatarUrl = m.User.AvatarUrl,
                    JobTitle = m.User.JobTitle
                })
                .ToList();

            Console.WriteLine($"[DEBUG] Accepted members count: {acceptedMembers.Count}");

            var project = await _context.Projects
                .Include(p => p.Owner)
                .FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null)
            {
                Console.WriteLine("[DEBUG] Project not found.");
                return acceptedMembers;
            }

            if (project.Owner != null)
            {
                Console.WriteLine($"[DEBUG] Project Owner ID = {project.Owner.Id}, Name = {project.Owner.Username}");

                if (!acceptedMembers.Any(u => u.Id == project.OwnerId))
                {
                    acceptedMembers.Add(new SimpleUserDto
                    {
                        Id = project.Owner.Id,
                        Username = project.Owner.Username,
                        FullName = project.Owner.FullName,
                        Email = project.Owner.Email,
                        AvatarUrl = project.Owner.AvatarUrl,
                        JobTitle = project.Owner.JobTitle
                    });

                    Console.WriteLine("[DEBUG] Owner added to the list.");
                }
                else
                {
                    Console.WriteLine("[DEBUG] Owner already in the list.");
                }
            }
            else
            {
                Console.WriteLine("[DEBUG] Project.Owner is null!!");
            }

            return acceptedMembers;
        }



        public async Task<IEnumerable<User>> GetAllUsersInProject(Guid projectId)
        {
            var team = await GetTeamByProjectId(projectId);
            if (team == null)
                return Enumerable.Empty<User>();

            var users = await _context.Invitations
                .Where(i => i.TeamId == team.Id && i.Status == InvitationStatus.Accepted && i.Invitee != null)
                .Include(i => i.Invitee)
                .Select(i => i.Invitee!)
                .ToListAsync();

            return users;
        }



    }
}
