using Microsoft.EntityFrameworkCore;
using OptiPlanBackend.Data;
using OptiPlanBackend.Models;
using OptiPlanBackend.Repositories.Interfaces;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OptiPlanBackend.Repositories.Implementations
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {

        private readonly UserDbContext _context;

        public UserRepository(UserDbContext context) : base(context)
        {

            _context = context;
        }



        public async Task<IEnumerable<User>> GetTeamByProjectId(Guid projectId)
        {
            var users = await _context.TeamMemberships
                .Where(tm => tm.Team.ProjectId == projectId)
                .Include(tm => tm.User) // important pour charger les Users
                .Select(tm => tm.User)
                .ToListAsync();

            return users;
        }
    }
}
