using OptiPlanBackend.Models;

namespace OptiPlanBackend.Services.Interfaces
{
    public interface IDirectMessageService
    {
        Task<IEnumerable<DirectMessage>> GetAllAsync();
        Task<DirectMessage?> GetByIdAsync(Guid id);
        Task<bool> CreateAsync(DirectMessage directMessage);
        Task<bool> UpdateAsync(DirectMessage directMessage);
        Task<bool> DeleteAsync(DirectMessage directMessage);



        public Task<List<DirectMessage>> GetMessagesAsync(Guid chatId);
    }
}
