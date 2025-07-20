using OptiPlanBackend.Extensions;
using OptiPlanBackend.Services.Interfaces;
using System.Security.Claims;

namespace OptiPlanBackend.Services.Implementations
{
   

    public class CurrentUserService : ICurrentUserService
    {
        public Guid? UserId { get; }
        public string? Email { get; }

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            UserId = httpContextAccessor.HttpContext?.User.GetUserId();
            Email = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email);
        }
    }
}
