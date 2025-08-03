using OptiPlanBackend.Dto;
using OptiPlanBackend.Enums;
using OptiPlanBackend.Models;

namespace OptiPlanBackend.Repositories.Interfaces
{

   
        public interface IWorkItemRepository : IGenericRepository<WorkItem>
        {
            Task<IEnumerable<IGrouping<(Guid ProjectId, string ProjectTitle), WorkItem>>>
                GetUserTasksGroupedByProjectForMonth(Guid userId, int month, int year);



            public Task<IEnumerable<WorkItem>> GetWorkItemByProjectIdAsync(Guid projectId);



            public Task<WorkItem> AddWorkItemForAProject(ProjectTaskDto projectTaskDto, Guid userId);

            public Task<bool> UpdateWorkItemStatusAsync(Guid workItemId, WorkItemStatus newStatus);

    }

}

