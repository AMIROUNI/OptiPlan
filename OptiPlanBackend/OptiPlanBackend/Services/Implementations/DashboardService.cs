using Microsoft.EntityFrameworkCore;
using OptiPlanBackend.Data;
using OptiPlanBackend.Dto;
using OptiPlanBackend.Enums;
using OptiPlanBackend.Services.Interfaces;

namespace OptiPlanBackend.Services.Implementations
{
    public class DashboardService : IDashboardService
    {

        private readonly UserDbContext _context;

        public DashboardService(UserDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<List<KpiDto>> GetUserKpis(Guid userId)
        {
            var user = await _context.Users
                .Include(u => u.OwnedProjects)
                .Include(u => u.TeamMemberships)
                    .ThenInclude(tm => tm.Team)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new KeyNotFoundException("User not found");

            return new List<KpiDto>
            {
                // 1. Project Metrics
                new KpiDto
                {
                    Title = "Total Projects",
                    Value = user.OwnedProjects.Count + user.TeamMemberships.Count(tm => tm.Status == MembershipStatus.Active)
                },
                
                // 2. Task Metrics (assuming you have a Tasks table)
                new KpiDto
                {
                    Title = "Active Tasks",
                    Value = await _context.Tasks.CountAsync(t => t.AssignedUserId == userId && !t.IsCompleted)
                },
                
                // 3. Pending Invitations
                new KpiDto
{
    Title = "Pending Invites",
    Value = await _context.Invitations
        .CountAsync(i => (i.Email == user.Email || i.InviteeId == userId)
                      && i.Status == InvitationStatus.Pending)
},  
                
                // 4. Team Members
                new KpiDto
                {
                    Title = "Team Members",
                    Value = user.TeamMemberships
                        .Select(tm => tm.Team)
                        .Distinct()
                        .Sum(team => team.Members.Count)
                },
                
                // 5. Overdue Tasks (example)
                new KpiDto
                {
                    Title = "Overdue Tasks",
                    Value = await _context.Tasks
                        .CountAsync(t => t.AssignedUserId == userId &&
                                       t.DueDate < DateTime.UtcNow &&
                                       !t.IsCompleted)
                }
            };
        }
    }
}

