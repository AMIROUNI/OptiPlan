using OptiPlanBackend.Models;
using System.IdentityModel.Tokens.Jwt;

namespace OptiPlanBackend.Services.Interfaces
{
    public interface IUserService
    {
        public Task<User?> GetUserByIdAsync(Guid userId);
        public Task<User?> GetUserByTokenAsync(string token);
    }
}