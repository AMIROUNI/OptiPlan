using OptiPlanBackend.Models;

namespace OptiPlanBackend.Services.Interfaces
{
    public interface IUserProfileService
    {

        Task<IEnumerable<UserProfile>> GetAllAsync();
        Task<UserProfile?> GetByIdAsync(Guid id);
        Task<bool> CreateAsync(UserProfile userProfile);
        Task<bool> UpdateAsync(UserProfile userProfile);
        Task<bool> DeleteAsync(UserProfile userProfile);
        Task<UserProfile> GetUserByIdAsync(Guid userId);

 
    }
}
 