using OptiPlanBackend.Models;

namespace OptiPlanBackend.Repositories.Interfaces
{
    public interface ICommentRepository : IGenericRepository<Comment>
    {
        public  Task<IEnumerable<Comment>> GetCommentsByWorkItemIdAsync(Guid workItemId);
    }
    
 }
