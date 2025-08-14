using OptiPlanBackend.Models;
using OptiPlanBackend.Repositories.Interfaces;
using OptiPlanBackend.Services.Interfaces;

namespace OptiPlanBackend.Services.Implementations
{
    public class DirectChatService : IDirectChatService
    {
        private readonly IDirectChatRepository _directChatRepository;
        public DirectChatService(IDirectChatRepository directChatRepository)
        {
            _directChatRepository = directChatRepository;
        }
        public async Task<bool> CreateAsync(DirectChat directChat)
        {
            await _directChatRepository.AddAsync(directChat);
            return await _directChatRepository.SaveChangesAsync();
        }

        public Task<DirectChat> CreatePrivateChatAsync(Guid user1Id, Guid user2Id)
        { 
            return _directChatRepository.CreatePrivateChatAsync(user1Id, user2Id);
        }

        public async Task<bool> DeleteAsync(DirectChat directChat)
        {
             _directChatRepository.Delete(directChat);
            return await _directChatRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<DirectChat>> GetAllAsync()
        {
          return await _directChatRepository.GetAllAsync();
        }

        public async Task<DirectChat?> GetByIdAsync(Guid id)
        {
           return await _directChatRepository.GetByIdAsync(id);
        }

        public Task<DirectChat?> GetPrivateChatAsync(Guid user1Id, Guid user2Id)
        {
            return _directChatRepository.GetPrivateChatAsync(user1Id, user2Id);
        }

        public async Task<bool> UpdateAsync(DirectChat directChat)
        {
             _directChatRepository.Update(directChat);
            return await _directChatRepository.SaveChangesAsync();
        }
    }
}
