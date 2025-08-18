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
         
        public async Task<User> findUserByUsername(string username)
        {
            return (User) await FindAsync(u => u.Username == username);
        }



        public async Task<IEnumerable<User>> getAllUserNotADMIN()
        {
            return  await _context.Users.Where(u => u.Role != Enums.Role.Admin)
                .ToListAsync();
        }

        public async Task<User> GetUserByUsernameAsync(string name)
        {
           return await _context.Users
                .FirstOrDefaultAsync(u => u.Username == name);
        }
    }
}
