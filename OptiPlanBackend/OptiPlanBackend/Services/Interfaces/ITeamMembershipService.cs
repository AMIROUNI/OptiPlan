using OptiPlanBackend.Models;

namespace OptiPlanBackend.Services.Interfaces
{
    public interface ITeamMembershipService
    {

        Task<IEnumerable<TeamMembership>> GetByProjectIdAsync(Guid projectId);
        Task<IEnumerable<TeamMembership>> GetByUserIdAsync(Guid userId);
        Task<TeamMembership?> GetByUserAndProjectIdAsync(Guid userId, Guid projectId);


        Task<IEnumerable<TeamMembership>> GetAllAsync();
        Task<TeamMembership?> GetByIdAsync(Guid id);
        Task<bool> CreateAsync(TeamMembership team);
        Task<bool> UpdateAsync(TeamMembership team);
        Task<bool> DeleteAsync(TeamMembership team);


    }
}
