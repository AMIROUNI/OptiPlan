
    using global::OptiPlanBackend.Data;
    using global::OptiPlanBackend.Enums;
    using global::OptiPlanBackend.Models;
    using global::OptiPlanBackend.Repositories.Interfaces;
    using Microsoft.EntityFrameworkCore;
using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    namespace OptiPlanBackend.Repositories.Implementations
    {
        public class WorkItemRepository : GenericRepository<WorkItem>, IWorkItemRepository
        {
            private readonly UserDbContext _context;

            public WorkItemRepository(UserDbContext context) : base(context)
            {
                _context = context;
            }

            public async Task<IEnumerable<IGrouping<(Guid ProjectId, string ProjectTitle), WorkItem>>>
            GetUserTasksGroupedByProjectForMonth(Guid userId, int month, int year)
            {
                var tasks = await _context.WorkItems
                    .Include(t => t.Project)
                    .Where(t =>
                        t.AssignedUserId == userId &&
                        t.CreatedAt.Month == month &&
                        t.CreatedAt.Year == year)
                    .ToListAsync();

                // Group by project ID and title
                return tasks.GroupBy(t => (t.ProjectId, t.Project.Title));
            }



           
        


        public async Task<IEnumerable<WorkItem>> GetProjectTasksByProjectIdAsync(Guid projectId)
            {
                return await FindAsync(pt => pt.ProjectId == projectId);

            }

            public async Task<WorkItem> AddWorkItemForAProject(Dto.ProjectTaskDto projectTaskDto, Guid userId)
            {
                var projectTask = new WorkItem
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
                    Status = Enums.WorkItemStatus.ToDo,
                    CompletionPercentage = 0
                };

                await AddAsync(projectTask);
                await SaveChangesAsync();

                return projectTask;
            }

        public async Task<bool> UpdateWorkItemStatusAsync(Guid workItemId, WorkItemStatus newStatus)
        {
            var workItem = await _context.WorkItems.FindAsync(workItemId);
            if (workItem == null)
                return false;

            workItem.Status = newStatus;

            if (newStatus == WorkItemStatus.Done)
                workItem.CompletedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
    }

    }


