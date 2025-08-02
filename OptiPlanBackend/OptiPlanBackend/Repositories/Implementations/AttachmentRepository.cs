using Microsoft.EntityFrameworkCore;
using OptiPlanBackend.Data;
using OptiPlanBackend.Models;
using OptiPlanBackend.Repositories.Interfaces;

namespace OptiPlanBackend.Repositories.Implementations
{
    public class AttachmentRepository : GenericRepository<Attachment>, IAttachmentRepository
    {

        private readonly UserDbContext _context;
        public AttachmentRepository(UserDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Attachment> GetAttachmentByWorkItemIdAndFileNameAndUploaderId(Guid workItemId, string fileName, Guid uploaderId)
        {
           return await _context.Attachments
                        .FirstOrDefaultAsync(a =>
                            a.WorkItemId == workItemId &&
                            a.FileName == fileName &&
                            a.UploaderId == uploaderId);
        }

        public async Task<IEnumerable<Attachment>> GetByAttachmentsByWorkItemIdAsync(Guid workItemId)
        {
            return await FindAsync(a => a.WorkItemId == workItemId);

        }
    }
}
