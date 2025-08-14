using OptiPlanBackend.Models;
using OptiPlanBackend.Repositories.Interfaces;
using OptiPlanBackend.Services.Interfaces;

namespace OptiPlanBackend.Services.Implementations
{
    public class DirectMessageService : IDirectMessageService
    {

        private readonly IDirectMessageRepository _directMessageRepository;
        public async Task<bool> CreateAsync(DirectMessage directMessage)
        {
           await _directMessageRepository.AddAsync(directMessage);
            return await _directMessageRepository.SaveChangesAsync();
        }

        public  async Task<bool> DeleteAsync(DirectMessage directMessage)
        {
             _directMessageRepository.Delete(directMessage);
            return await _directMessageRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<DirectMessage>> GetAllAsync()
        {
           return await _directMessageRepository.GetAllAsync();
        }

        public Task<DirectMessage?> GetByIdAsync(Guid id)
        {
            return _directMessageRepository.GetByIdAsync(id);
        }

        public async Task<List<DirectMessage>> GetMessagesAsync(Guid chatId)
        {
           return await _directMessageRepository.GetMessagesAsync(chatId);
        }

        public async Task<bool> UpdateAsync(DirectMessage directMessage)
        {
             _directMessageRepository.Update(directMessage);
            return await _directMessageRepository.SaveChangesAsync();
        }
    }
}
