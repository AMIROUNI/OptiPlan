using OptiPlanBackend.Models;

namespace OptiPlanBackend.Repositories.Interfaces
{
    public interface IWorkItemHistoryRepository : IGenericRepository<WorkItemHistory>
    {

        public Task<IEnumerable<WorkItemHistory>> GetByWorkItemHistorysByWorkItemIdAsync(Guid workItemId);


    }
    
 }

