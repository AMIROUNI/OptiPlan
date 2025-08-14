using Microsoft.EntityFrameworkCore;
using OptiPlanBackend.Data;
using OptiPlanBackend.Models;
using OptiPlanBackend.Repositories.Interfaces;

namespace OptiPlanBackend.Repositories.Implementations
{
    public class UserProfileRepository : GenericRepository<UserProfile>, IUserProfileRepository
    {

        private readonly UserDbContext _context;
        public UserProfileRepository(UserDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<UserProfile?> GetUserByIdAsync(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentException("User ID cannot be empty", nameof(userId));
            }

            return await _context.UserProfiles
                .Include(up => up.Skills)
                .AsNoTracking()
                .FirstOrDefaultAsync(up => up.UserId == userId)
                .ConfigureAwait(false);
        }
    }
}
