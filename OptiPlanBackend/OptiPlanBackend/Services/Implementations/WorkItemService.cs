using Microsoft.VisualBasic;
using OptiPlanBackend.Dto;
using OptiPlanBackend.Enums;
using OptiPlanBackend.Models;
using OptiPlanBackend.Repositories.Implementations;
using OptiPlanBackend.Repositories.Interfaces;
using OptiPlanBackend.Services.Interfaces;


namespace OptiPlanBackend.Services.Implementations
{
    public class WorkItemService : IWorkItemService
    {

        private readonly IWorkItemRepository _workItemRepository;

        public WorkItemService(IWorkItemRepository taskRepository)
        {
            _workItemRepository = taskRepository;
        }


        public async Task<bool> CreateAsync(OptiPlanBackend.Models.WorkItem task)
        {
            await _workItemRepository.AddAsync(task);
            return await _workItemRepository.SaveChangesAsync();

        }

        public Task<bool> DeleteAsync(WorkItem project)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<WorkItem>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<WorkItem?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<WorkItem>> GetByOwnerIdAsync(Guid ownerId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProjectTaskCountDto>> GetMonthlyTasksByProjectAsync(Guid userId, int month, int year)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(WorkItem task)
        {
            throw new NotImplementedException();
        }

        async Task<WorkItem?> IWorkItemService.GetByIdAsync(Guid id)
        {
            return await _workItemRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<IGrouping<(Guid ProjectId, string ProjectTitle), WorkItem>>>
           GetUserTasksGroupedByProjectForMonth(Guid userId, int month, int year)
        {

            return await _workItemRepository.GetUserTasksGroupedByProjectForMonth(userId, month, year);
        }

        public async Task<IEnumerable<WorkItem>> GetProjectTasksByProjectIdAsync(Guid projectId)
        {
            return await _workItemRepository.GetProjectTasksByProjectIdAsync(projectId);

        }

        public async Task<WorkItem> AddWorkItemForAProject(ProjectTaskDto projectTaskDto, Guid userId)
        {
          return await _workItemRepository.AddWorkItemForAProject(projectTaskDto,userId);
        }

        public async Task<bool> UpdateWorkItemStatusAsync(Guid workItemId, WorkItemStatus newStatus)
        {
           return await _workItemRepository.UpdateWorkItemStatusAsync(workItemId, newStatus);
        }
    }
}
