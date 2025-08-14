using OptiPlanBackend.Models;
using OptiPlanBackend.Repositories.Interfaces;
using OptiPlanBackend.Services.Interfaces;

namespace OptiPlanBackend.Services.Implementations
{
    public class SkillService : ISkillService
    {
        private readonly ISkillRepository _skillRepository;

        public SkillService(ISkillRepository skillRepository)
        {
            _skillRepository = skillRepository;
        }

        public async Task<bool> CreateAsync(Skill userProfile)
        {
            await _skillRepository.AddAsync(userProfile);
            return await _skillRepository.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Skill userProfile)
        {
             _skillRepository.Delete(userProfile);
            return await _skillRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<Skill>> GetAllAsync()
        {
            return await _skillRepository.GetAllAsync();
        }

        public Task<Skill?> GetByIdAsync(Guid id)
        {
             return _skillRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Skill>> GetUserSkillsByUserIdAsync(Guid userId)
        {
             return await _skillRepository.GetUserSkillsByUserIdAsync(userId);
        }

        public  async Task<bool> UpdateAsync(Skill userProfile)
        {
             _skillRepository.Update(userProfile);
            return await _skillRepository.SaveChangesAsync();
        }
    }
}
