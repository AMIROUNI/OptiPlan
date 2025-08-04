using OptiPlanBackend.Models;
using OptiPlanBackend.Repositories.Interfaces;
using OptiPlanBackend.Services.Interfaces;

namespace OptiPlanBackend.Services.Implementations
{
    public class ChatMessageService : IChatMessageService
    {

        private readonly  IChatMessageRepository _chatMessageRepository;
        public ChatMessageService(IChatMessageRepository chatMessageRepository)
        {
            _chatMessageRepository = chatMessageRepository;
        }

        public async Task<bool> CreateAsync(ChatMessage chatMessage)
        {
             await  _chatMessageRepository.AddAsync(chatMessage);
            return await _chatMessageRepository.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(ChatMessage chatMessage)
        {
             _chatMessageRepository.Delete(chatMessage);
            return await _chatMessageRepository.SaveChangesAsync();
        }

        public Task<IEnumerable<ChatMessage>> GetAllAsync()
        {
            return _chatMessageRepository.GetAllAsync();
        }

        public async Task<ChatMessage?> GetByIdAsync(Guid id)
        {
             return await  _chatMessageRepository.GetByIdAsync(id);
        }

        public async Task<bool> UpdateAsync(ChatMessage chatMessage)
        {
            _chatMessageRepository.Update(chatMessage);
            return await _chatMessageRepository.SaveChangesAsync();
        }
    }
}
