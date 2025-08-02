using OptiPlanBackend.Enums;
using OptiPlanBackend.Models;

namespace OptiPlanBackend.Services.Interfaces
{
    public interface ITeamService
    {

        Task<IEnumerable<Team>> GetAllAsync();
        Task<Team?> GetByIdAsync(Guid id);
        Task<bool> CreateAsync(Team team);
        Task<bool> UpdateAsync(Team team);
        Task<bool> DeleteAsync(Team team);

        Task<TeamRole?> GetUserRoleInProjectAsync(Guid userId, Guid projectId);
    }
}
