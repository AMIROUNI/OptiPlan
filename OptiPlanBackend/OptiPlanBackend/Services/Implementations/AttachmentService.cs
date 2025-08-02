using OptiPlanBackend.Models;
using OptiPlanBackend.Repositories.Interfaces;
using OptiPlanBackend.Services.Implementations;
using OptiPlanBackend.Services.Interfaces;
using System.Net.NetworkInformation;

namespace OptiPlanBackend.Services.Implementations
{
    public class AttachmentService : IAttachmentService
    {

        private readonly IAttachmentRepository _attachmentRepository;

        public AttachmentService(IAttachmentRepository attachmentRepository)
        {
            _attachmentRepository = attachmentRepository;
        }

        public async Task<bool> CreateAsync(Attachment invitation)
        {
            await _attachmentRepository.AddAsync(invitation);
            return await _attachmentRepository.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Attachment invitation)
        {
             _attachmentRepository.Delete(invitation);
            return await _attachmentRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<Attachment>> GetAllAsync()
        {
            return await _attachmentRepository.GetAllAsync();
        }

        public async Task<Attachment?> GetByIdAsync(Guid id)
        {
            return await _attachmentRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Attachment>> GetByAttachmentsByWorkItemIdAsync(Guid workItemId)
        {
            return await _attachmentRepository.GetByAttachmentsByWorkItemIdAsync(workItemId);
        }

        public async Task<bool> UpdateAsync(Attachment invitation)
        {
             _attachmentRepository.Update(invitation);
            return await _attachmentRepository.SaveChangesAsync();
        }

        public async Task<Attachment> GetAttachmentByWorkItemIdAndFileNameAndUploaderId(Guid workItemId, string fileName, Guid uploaderId)
        {
            return await _attachmentRepository.GetAttachmentByWorkItemIdAndFileNameAndUploaderId(
                workItemId, fileName, uploaderId);  
        }

    }
}
