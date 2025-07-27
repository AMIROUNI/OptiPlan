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
            _context = context;
        }

        public async Task<List<KpiDto>> GetUserKpis(Guid userId)
        {
            // First verify user exists
            var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
            if (!userExists)
                throw new KeyNotFoundException("User not found");

            // Run KPI methods sequentially to avoid concurrent DbContext access
            var totalProjects = await GetTotalProjects(userId);
            var activeTasks = await GetActiveTasks(userId);
            var pendingInvites = await GetPendingInvites(userId);
            var teamMembers = await GetTeamMembers(userId);
            var overdueTasks = await GetOverdueTasks(userId);

            return new List<KpiDto>
    {
        new() { Title = "Total Projects", Value = totalProjects },
        new() { Title = "Active Tasks", Value = activeTasks },
        new() { Title = "Pending Invites", Value = pendingInvites },
        new() { Title = "Team Members", Value = teamMembers },
        new() { Title = "Overdue Tasks", Value = overdueTasks }
    };
        }


        private async Task<int> GetTotalProjects(Guid userId)
        {
            var ownedProjects = await _context.Projects
                .CountAsync(p => p.OwnerId == userId);

            var teamProjects = await _context.TeamMemberships
                .CountAsync(tm => tm.UserId == userId &&
                               tm.Status == MembershipStatus.Active);

            return ownedProjects + teamProjects;
        }

        private async Task<int> GetActiveTasks(Guid userId)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var count = await _context.WorkItems
                .CountAsync(w => w.AssignedUserId == userId && w.Status != WorkItemStatus.Done);

                await transaction.CommitAsync();
                return count;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        private async Task<int> GetPendingInvites(Guid userId)
        {
            // Get user email first
            var userEmail = await _context.Users
                .Where(u => u.Id == userId)
                .Select(u => u.Email)
                .FirstOrDefaultAsync();

            // Count pending invites matching user's email OR user ID
            return await _context.Invitations
                .CountAsync(i => (i.Email == userEmail || i.InviteeId == userId) &&
                               i.Status == InvitationStatus.Pending);
        }

        private async Task<int> GetTeamMembers(Guid userId)
        {
            return await _context.TeamMemberships
                .Where(tm => _context.TeamMemberships
                    .Where(innerTm => innerTm.UserId == userId)
                    .Select(innerTm => innerTm.TeamId)
                    .Distinct()
                    .Contains(tm.TeamId))
                .CountAsync();
        }
        private async Task<int> GetOverdueTasks(Guid userId)
        {
            // Count tasks assigned to user that are overdue
            return await _context.WorkItems
                .CountAsync(w =>w.AssignedUserId == userId &&
                               w.DueDate < DateTime.UtcNow &&
                               w.Status != WorkItemStatus.Done);
        }
    }
}