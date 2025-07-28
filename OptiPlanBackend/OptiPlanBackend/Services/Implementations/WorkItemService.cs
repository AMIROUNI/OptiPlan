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

        public async Task<bool> DeleteAsync(WorkItem workItem)
        {
             _workItemRepository.Delete(workItem);
            return await _workItemRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<WorkItem>> GetAllAsync()
        {
            return await _workItemRepository.GetAllAsync();
        }

        public async Task<WorkItem?> GetByIdAsync(Guid id)
        {
            return await _workItemRepository.GetByIdAsync(id);
        }

        public Task<IEnumerable<WorkItem>> GetByOwnerIdAsync(Guid ownerId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProjectTaskCountDto>> GetMonthlyTasksByProjectAsync(Guid userId, int month, int year)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAsync(WorkItem task)
        {
            _workItemRepository.Update(task);
            return await _workItemRepository.SaveChangesAsync();
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
