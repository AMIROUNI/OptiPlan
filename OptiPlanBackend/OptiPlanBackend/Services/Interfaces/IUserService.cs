using OptiPlanBackend.Models;
using System.IdentityModel.Tokens.Jwt;

namespace OptiPlanBackend.Services.Interfaces
{
    public interface IUserService
    {
        public Task<User?> GetUserByIdAsync(Guid userId);
        public Task<User?> GetUserByTokenAsync(string token);
        Task<bool> UpdateAsync(User user);
        Task<bool> DeleteAsync(User user);

        public  Task<IEnumerable<User>> GetTeamByProjectId(Guid projectId);
        public  Task<IEnumerable<User>> getAllUserNotADMIN();
        
    }
}