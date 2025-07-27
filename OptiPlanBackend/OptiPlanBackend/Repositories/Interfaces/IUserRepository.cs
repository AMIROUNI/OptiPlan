using OptiPlanBackend.Models;

namespace OptiPlanBackend.Repositories.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {

        public Task<IEnumerable<User>> GetTeamByProjectId(Guid projectId);
    }
}
