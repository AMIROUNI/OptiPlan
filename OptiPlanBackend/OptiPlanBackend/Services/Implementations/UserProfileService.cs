using OptiPlanBackend.Models;
using OptiPlanBackend.Repositories.Interfaces;
using OptiPlanBackend.Services.Interfaces;

namespace OptiPlanBackend.Services.Implementations
{
    public class UserProfileService : IUserProfileService
    {

        private readonly IUserProfileRepository _userProfileRepository;

        public UserProfileService(IUserProfileRepository userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
        }

        public async Task<bool> CreateAsync(UserProfile userProfile)
        {
            await _userProfileRepository
            .AddAsync(userProfile);
            return await _userProfileRepository.SaveChangesAsync();
        }

        public  async Task<bool> DeleteAsync(UserProfile userProfile)
        {
             _userProfileRepository
           .Delete(userProfile);
            return await _userProfileRepository.SaveChangesAsync();
        }

        public Task<IEnumerable<UserProfile>> GetAllAsync()
        {
            return _userProfileRepository.GetAllAsync();
        }

        public Task<UserProfile?> GetByIdAsync(Guid id)
        {
            return _userProfileRepository.GetByIdAsync(id);
        }

        public async Task<UserProfile> GetUserByIdAsync(Guid userId)
        {
            return await _userProfileRepository.GetUserByIdAsync(userId);
        }

        public async Task<bool> UpdateAsync(UserProfile userProfile)
        {
            _userProfileRepository
           .Update(userProfile);
            return await _userProfileRepository.SaveChangesAsync();

        }
    }
}
