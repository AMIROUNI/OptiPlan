using OptiPlanBackend.Models;

namespace OptiPlanBackend.Repositories.Interfaces
{
    public interface ISkillRepository : IGenericRepository<Skill>
    {

        public  Task<IEnumerable<Skill>> GetUserSkillsByUserIdAsync(Guid userId);
    }
}
