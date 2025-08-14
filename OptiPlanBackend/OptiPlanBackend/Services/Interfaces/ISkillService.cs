using OptiPlanBackend.Models;

namespace OptiPlanBackend.Services.Interfaces
{
    public interface ISkillService
    {

        Task<IEnumerable<Skill>> GetAllAsync();
        Task<Skill?> GetByIdAsync(Guid id);
        Task<bool> CreateAsync(Skill userProfile);
        Task<bool> UpdateAsync(Skill userProfile);
        Task<bool> DeleteAsync(Skill userProfile);
        public  Task<IEnumerable<Skill>> GetUserSkillsByUserIdAsync(Guid userId);
    }
}
