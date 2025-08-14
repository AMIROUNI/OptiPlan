using Microsoft.EntityFrameworkCore;
using OptiPlanBackend.Data;
using OptiPlanBackend.Models;
using OptiPlanBackend.Repositories.Interfaces;

namespace OptiPlanBackend.Repositories.Implementations
{
    public class SkillRepository : GenericRepository<Skill>, ISkillRepository
    {
        private readonly UserDbContext _context;
        public SkillRepository(UserDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Skill>> GetUserSkillsByUserIdAsync(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentException("User ID cannot be empty", nameof(userId));
            }

            return await _context.UserProfiles
                .Where(up => up.UserId == userId)
                .Include(up => up.Skills)
                .SelectMany(up => up.Skills)
                .AsNoTracking()
                .ToListAsync()
                .ConfigureAwait(false);
        }
    }
}
