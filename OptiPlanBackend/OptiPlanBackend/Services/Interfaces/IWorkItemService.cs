using OptiPlanBackend.Dto;
using OptiPlanBackend.Enums;
using OptiPlanBackend.Models;





namespace OptiPlanBackend.Services.Interfaces
    {
        public interface IWorkItemService
        {

         Task<IEnumerable<OptiPlanBackend.Models.WorkItem>> GetAllAsync();
        Task<WorkItem?> GetByIdAsync(Guid id);
        Task<IEnumerable<OptiPlanBackend.Models.WorkItem>> GetByOwnerIdAsync(Guid ownerId);
        Task<bool> CreateAsync(OptiPlanBackend.Models.WorkItem project);
        Task<bool> UpdateAsync(OptiPlanBackend.Models.WorkItem project);
        Task<bool> DeleteAsync(OptiPlanBackend.Models.WorkItem project);

        public  Task<IEnumerable<IGrouping<(Guid ProjectId, string ProjectTitle), WorkItem>>>
           GetUserTasksGroupedByProjectForMonth(Guid userId, int month, int year);

        public Task<IEnumerable<WorkItem>> GetWorkItemByProjectIdAsync(Guid projectId);

        public  Task<WorkItem> AddWorkItemForAProject(Dto.ProjectTaskDto projectTaskDto,Guid userId);
        public Task<bool> UpdateWorkItemStatusAsync(Guid workItemId, WorkItemStatus newStatus);
    }
    }


