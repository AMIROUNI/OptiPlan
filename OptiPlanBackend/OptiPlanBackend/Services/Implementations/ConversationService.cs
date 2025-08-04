using OptiPlanBackend.Models;
using OptiPlanBackend.Repositories.Interfaces;
using OptiPlanBackend.Services.Interfaces;

namespace OptiPlanBackend.Services.Implementations
{
    public class ConversationService : IConversationService
    {
        private readonly IConversationRepository _conversationRepository;
        public ConversationService(IConversationRepository conversationRepository)
        {
            _conversationRepository = conversationRepository;
        }
        public async Task<bool> CreateAsync(Conversation conversation)
        {
             await _conversationRepository.AddAsync(conversation);
            return await _conversationRepository.SaveChangesAsync();
        }

        public  async Task<bool> DeleteAsync(Conversation conversation)
        {
             _conversationRepository.Delete(conversation);
            return await _conversationRepository.SaveChangesAsync();
        }

        public Task<IEnumerable<Conversation>> GetAllAsync()
        {
             return _conversationRepository.GetAllAsync();
        }

        public async Task<Conversation?> GetByIdAsync(Guid id)
        { 
            return await _conversationRepository.GetByIdAsync(id);
        }

        public async Task<Conversation?> GetConversationByIdIncludeMessages(Guid conversationId)
        {
            return await _conversationRepository.GetConversationByIdIncludeMessages(conversationId);
        }

        public async Task<IEnumerable<Conversation>> GetConversationByUserId(Guid userId)
        {
            return await _conversationRepository.GetConversationByUserId(userId);
        }

        public async Task<bool> UpdateAsync(Conversation conversation)
        {
             _conversationRepository.Update(conversation);
            return await _conversationRepository.SaveChangesAsync();
        }
    }
}
