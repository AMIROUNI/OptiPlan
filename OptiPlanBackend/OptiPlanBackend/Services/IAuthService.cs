using OptiPlanBackend.Models;
using OptiPlanBackend.Dto;

namespace OptiPlanBackend.Services
{
    public interface IAuthService
    {
        Task<User?> RegisterAsync(RegisterDto request); 

        Task<TokenResponseDto?> LoginAsync(UserDto request);
        Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request);
    }
}
