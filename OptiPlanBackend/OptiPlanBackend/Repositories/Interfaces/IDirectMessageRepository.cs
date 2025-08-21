using OptiPlanBackend.Dto;
using OptiPlanBackend.Models;

namespace OptiPlanBackend.Repositories.Interfaces
{
    public interface IDirectMessageRepository : IGenericRepository<DirectMessage>
    {

        public  Task<List<DirectMessage>> GetMessagesAsync(Guid chatId);

      public Task<List<MessageDto>> GetMessagesByChatIdAsync(Guid chatId);


    }
}
