using OptiPlanBackend.Dtos;
using OptiPlanBackend.Models;

namespace OptiPlanBackend.Repositories.Interfaces
{
    public interface ISprintRepository : IGenericRepository<Sprint>
    {

        public Task<IEnumerable<Sprint>> GetSprintsByProjectId(Guid projectId);
        public  Task<Sprint> CreateSprintForProject(Guid projectId, SprintDto sprintDto);

    }
}
