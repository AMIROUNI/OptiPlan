using OptiPlanBackend.Dto;
using OptiPlanBackend.Models;

namespace OptiPlanBackend.Repositories.Interfaces
{
    public interface IUserProfileRepository :IGenericRepository<UserProfile>
    {
        public  Task<UserProfile> GetUserByIdAsync(Guid userProfileId);
        public  Task<User> InitializeProfileAsync(Guid userId, InitializeProfileDto dto);
    }
}
