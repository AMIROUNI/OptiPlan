using OptiPlanBackend.Models;

namespace OptiPlanBackend.Services.Interfaces
{
    public interface IInvitationService
    {
        Task<IEnumerable<Invitation>> GetAllAsync();
        Task<Invitation?> GetByIdAsync(Guid id);
        Task<bool> CreateAsync(Invitation invitation);
        Task<bool> UpdateAsync(Invitation invitation);
        Task<bool> DeleteAsync(Invitation invitation);
        Task<Team> GetTeamWithProjectAsync(Guid teamId);
        Task<User> GetUserByIdAsync(Guid userId);
        public Task<IEnumerable<Invitation>> GetByUserIdAsync(Guid userId);
    }
}
