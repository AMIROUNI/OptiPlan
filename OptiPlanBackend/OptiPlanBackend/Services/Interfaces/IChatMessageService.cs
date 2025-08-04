using OptiPlanBackend.Models;

namespace OptiPlanBackend.Services.Interfaces
{
    public interface IChatMessageService
    {
        Task<IEnumerable<ChatMessage>> GetAllAsync();
        Task<ChatMessage?> GetByIdAsync(Guid id);
        Task<bool> CreateAsync(ChatMessage chatMessage);
        Task<bool> UpdateAsync(ChatMessage chatMessage);
        Task<bool> DeleteAsync(ChatMessage chatMessage);
    }
}
