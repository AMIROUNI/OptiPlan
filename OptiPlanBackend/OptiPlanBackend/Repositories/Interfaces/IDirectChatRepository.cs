using OptiPlanBackend.Models;

namespace OptiPlanBackend.Repositories.Interfaces
{
    public interface IDirectChatRepository : IGenericRepository<DirectChat>
    {

        Task<DirectChat?> GetPrivateChatAsync(Guid user1Id, Guid user2Id);
        Task<DirectChat> CreatePrivateChatAsync(Guid user1Id, Guid user2Id);
    }

}
