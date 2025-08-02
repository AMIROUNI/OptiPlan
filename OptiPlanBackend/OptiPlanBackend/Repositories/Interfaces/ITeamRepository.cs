using OptiPlanBackend.Enums;
using OptiPlanBackend.Models;

namespace OptiPlanBackend.Repositories.Interfaces
{
    public interface ITeamRepository : IGenericRepository<Team>
    {

        Task<TeamRole?> GetUserRoleInProjectAsync(Guid userId, Guid projectId);

    }
}
