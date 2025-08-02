using OptiPlanBackend.Models;

namespace OptiPlanBackend.Repositories.Interfaces

{
    public interface IAttachmentRepository : IGenericRepository<Attachment>
    {
        public Task<IEnumerable<Attachment>> GetByAttachmentsByWorkItemIdAsync(Guid workItemId);
        public Task<Attachment> GetAttachmentByWorkItemIdAndFileNameAndUploaderId(Guid workItemId,string fileName,
            Guid uploaderId);


    }
}
