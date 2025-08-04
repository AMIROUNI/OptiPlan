using OptiPlanBackend.Models;

namespace OptiPlanBackend.Services.Interfaces
{
    public interface IConversationService
    {
        Task<IEnumerable<Conversation>> GetAllAsync();
        Task<Conversation?> GetByIdAsync(Guid id);
        Task<bool> CreateAsync(Conversation conversation);
        Task<bool> UpdateAsync(Conversation conversation);
        Task<bool> DeleteAsync(Conversation conversation);



        public Task<Conversation?> GetConversationByIdIncludeMessages(Guid conversationId);
        public Task<IEnumerable<Conversation>> GetConversationByUserId(Guid userId);

    }
}
