namespace OptiPlanBackend.Repositories.Implementations
{
    using global::OptiPlanBackend.Data;
    using global::OptiPlanBackend.Models;
    using global::OptiPlanBackend.Repositories.Interfaces.OptiPlanBackend.Repositories.Interfaces;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    namespace OptiPlanBackend.Repositories.Implementations
    {
        public class TaskRepository : GenericRepository<ProjectTask>, ITaskRepository
        {
            private readonly UserDbContext _context;

            public TaskRepository(UserDbContext context) : base(context)
            {
                _context = context;
            }

            public async Task<IEnumerable<IGrouping<(Guid ProjectId, string ProjectTitle), ProjectTask>>>
            GetUserTasksGroupedByProjectForMonth(Guid userId, int month, int year)
            {
                var tasks = await _context.Tasks
                    .Include(t => t.Project)
                    .Where(t =>
                        t.AssignedUserId == userId &&
                        t.CreatedAt.Month == month &&
                        t.CreatedAt.Year == year)
                    .ToListAsync();

                // Group by project ID and title
                return tasks.GroupBy(t => (t.ProjectId, t.Project.Title));
            }
        }
    }

}
