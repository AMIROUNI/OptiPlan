using OptiPlanBackend.Models;

namespace OptiPlanBackend.Services.Interfaces
{
    public interface IAttachmentService
    {

        Task<IEnumerable<Attachment>> GetAllAsync();
        Task<Attachment?> GetByIdAsync(Guid id);
        Task<bool> CreateAsync(Attachment invitation);
        Task<bool> UpdateAsync(Attachment invitation);
        Task<bool> DeleteAsync(Attachment invitation);
   

        public Task<IEnumerable<Attachment>> GetByAttachmentsByWorkItemIdAsync(Guid workItemId);

        public Task<Attachment> GetAttachmentByWorkItemIdAndFileNameAndUploaderId(Guid workItemId, string fileName,
           Guid uploaderId);
    }
}
