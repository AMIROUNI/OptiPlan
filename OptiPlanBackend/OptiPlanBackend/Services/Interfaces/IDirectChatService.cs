using OptiPlanBackend.Models;

namespace OptiPlanBackend.Services.Interfaces
{
    public interface IDirectChatService
    {
        Task<IEnumerable<DirectChat>> GetAllAsync();
        Task<DirectChat?> GetByIdAsync(Guid id);
        Task<bool> CreateAsync(DirectChat directChat);
        Task<bool> UpdateAsync(DirectChat directChat);
        Task<bool> DeleteAsync(DirectChat directChat);


        Task<DirectChat?> GetPrivateChatAsync(Guid user1Id, Guid user2Id);
        Task<DirectChat> CreatePrivateChatAsync(Guid user1Id, Guid user2Id);
    }
}
