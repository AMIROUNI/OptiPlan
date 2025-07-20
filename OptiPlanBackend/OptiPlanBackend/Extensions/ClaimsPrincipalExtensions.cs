// Extensions/ClaimsPrincipalExtensions.cs
using System.Security.Claims;

namespace OptiPlanBackend.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid? GetUserId(this ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier) ??
                            user.FindFirst("sub"); // Fallback for JWT standard claim

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
                return null;

            return userId;
        }
    }
}