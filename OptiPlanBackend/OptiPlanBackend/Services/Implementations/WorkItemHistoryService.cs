using OptiPlanBackend.Models;
using OptiPlanBackend.Repositories.Interfaces;
using OptiPlanBackend.Services.Interfaces;

namespace OptiPlanBackend.Services.Implementations
{
    public class WorkItemHistoryService : IWorkItemHistoryService
    {

        private readonly IWorkItemHistoryRepository _workItemHistoryRepository;
        public WorkItemHistoryService(IWorkItemHistoryRepository workItemHistoryRepository)
        {
            _workItemHistoryRepository = workItemHistoryRepository;
        }
        public async Task<bool> CreateAsync(WorkItemHistory history)
        {
            await _workItemHistoryRepository.AddAsync(history);
            return await _workItemHistoryRepository.SaveChangesAsync();

        }

        public async Task<bool> DeleteAsync(WorkItemHistory history)
        {
             _workItemHistoryRepository.Delete(history);
            return await _workItemHistoryRepository.SaveChangesAsync();
        }

        public Task<IEnumerable<WorkItemHistory>> GetAllAsync()
        {
           return _workItemHistoryRepository.GetAllAsync();
        }

        public Task<WorkItemHistory?> GetByIdAsync(Guid id)
        {
           return _workItemHistoryRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<WorkItemHistory>> GetByWorkItemHistorysByWorkItemIdAsync(Guid workItemId)
        {

            return await _workItemHistoryRepository.GetByWorkItemHistorysByWorkItemIdAsync(workItemId);

        }

        public async Task<bool> UpdateAsync(WorkItemHistory history)
        {
             _workItemHistoryRepository.Update(history);
            return await _workItemHistoryRepository.SaveChangesAsync();
        }
    }
}
