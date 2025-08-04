using OptiPlanBackend.Models;

namespace OptiPlanBackend.Repositories.Interfaces
{
    public interface IConversationRepository : IGenericRepository<Conversation>
    {
        public Task<Conversation?> GetConversationByIdIncludeMessages(Guid conversationId);
        public Task<IEnumerable<Conversation> >  GetConversationByUserId(Guid userId);



        
    }
}
