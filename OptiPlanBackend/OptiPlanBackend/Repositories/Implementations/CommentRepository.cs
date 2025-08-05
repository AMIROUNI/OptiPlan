using Microsoft.EntityFrameworkCore;
using OptiPlanBackend.Data;
using OptiPlanBackend.Models;
using OptiPlanBackend.Repositories.Interfaces;

namespace OptiPlanBackend.Repositories.Implementations
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    { 
         private readonly UserDbContext _context;
        public CommentRepository(UserDbContext context) : base(context)
        {
            _context = context;
        }


        public  async Task<IEnumerable<Comment>> GetCommentsByWorkItemIdAsync(Guid workItemId)
        {
            return await  _context.Comments
                .Where(c=>c.WorkItemId == workItemId)
                .Include(c=>c.Author)
                .ToListAsync();
        }

    }
}
