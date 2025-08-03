using OptiPlanBackend.Data;
using OptiPlanBackend.Models;
using OptiPlanBackend.Repositories.Interfaces;

namespace OptiPlanBackend.Repositories.Implementations
{
    public class WorkItemHistoryRepository : GenericRepository<WorkItemHistory>, IWorkItemHistoryRepository
    {

        private readonly UserDbContext _context;
        public WorkItemHistoryRepository(UserDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<WorkItemHistory>> GetByWorkItemHistorysByWorkItemIdAsync(Guid workItemId)
        {
            return await FindAsync(wh=> wh.WorkItemId == workItemId);
        }
    }


}
