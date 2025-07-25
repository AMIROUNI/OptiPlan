namespace OptiPlanBackend.Repositories.Implementations
{
    using global::OptiPlanBackend.Data;
    using global::OptiPlanBackend.Enums;
    using global::OptiPlanBackend.Models;
    using global::OptiPlanBackend.Repositories.Interfaces.OptiPlanBackend.Repositories.Interfaces;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
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



           
        


        public async Task<IEnumerable<ProjectTask>> GetProjectTasksByProjectIdAsync(Guid projectId)
            {
                return await FindAsync(pt => pt.ProjectId == projectId);

            }

            public async Task<ProjectTask> AddProjectTaskForAProject(Dto.ProjectTaskDto projectTaskDto, Guid userId)
            {
                var projectTask = new ProjectTask
                {
                    Title = projectTaskDto.Title,
                    Description = projectTaskDto.Description,
                    ProjectId = projectTaskDto.ProjectId,
                    AssignedUserId = projectTaskDto.AssignedUserId,
                    ReporterId = userId,
                    Priority = projectTaskDto.Priority,
                    Type = projectTaskDto.Type,
                    DueDate = projectTaskDto.DueDate,
                    StartDate = projectTaskDto.StartDate,
                    EstimatedHours = projectTaskDto.EstimatedHours,
                    StoryPoints = projectTaskDto.StoryPoints,
                    Labels = projectTaskDto.Labels,
                    IsBlocked = projectTaskDto.IsBlocked,
                    BlockReason = projectTaskDto.BlockReason,
                    CreatedAt = DateTime.UtcNow,
                    Status = Enums.TaskStatus.ToDo,
                    CompletionPercentage = 0
                };

                await AddAsync(projectTask);
                await SaveChangesAsync();

                return projectTask;
            }

        }

    }

}
