using OptiPlanBackend.Models;

namespace OptiPlanBackend.Services.Interfaces
{
    public interface IWorkItemHistoryService
    {

        Task<IEnumerable<WorkItemHistory>> GetAllAsync();
        Task<WorkItemHistory?> GetByIdAsync(Guid id);
        Task<bool> CreateAsync(WorkItemHistory history);
        Task<bool> UpdateAsync(WorkItemHistory history);
        Task<bool> DeleteAsync(WorkItemHistory history);


        public Task<IEnumerable<WorkItemHistory>> GetByWorkItemHistorysByWorkItemIdAsync(Guid workItemId);

    }
}
