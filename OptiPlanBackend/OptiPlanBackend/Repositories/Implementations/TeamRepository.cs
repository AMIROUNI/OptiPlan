using OptiPlanBackend.Data;
using OptiPlanBackend.Models;
using OptiPlanBackend.Repositories.Interfaces;

namespace OptiPlanBackend.Repositories.Implementations
{
    public class TeamRepository : GenericRepository<Team>, ITeamRepository
    {

        private readonly UserDbContext _context;
        public TeamRepository(UserDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
