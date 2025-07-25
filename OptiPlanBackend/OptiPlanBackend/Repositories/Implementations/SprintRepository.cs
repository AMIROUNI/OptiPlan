using Microsoft.EntityFrameworkCore;
using OptiPlanBackend.Data;
using OptiPlanBackend.Dtos;
using OptiPlanBackend.Models;
using OptiPlanBackend.Repositories.Interfaces;

namespace OptiPlanBackend.Repositories.Implementations
{
    public class SprintRepository : GenericRepository<Sprint>, ISprintRepository
    { 
        private readonly UserDbContext _context;
        public SprintRepository(UserDbContext context) : base(context)
        { 
             _context = context;
        }



        public async Task<Sprint> CreateSprintForProject(Guid projectId, SprintDto sprintDto)
        {
            var sprint = new Sprint
            {
                ProjectId = projectId,
                Name = sprintDto.Name,
                Description = sprintDto.Description ?? string.Empty,
                StartDate = sprintDto.StartDate,
                EndDate = sprintDto.EndDate
            };

            await _context.Sprints.AddAsync(sprint);
            await _context.SaveChangesAsync();
            return sprint;
        }




        public async Task<IEnumerable<Sprint >> GetSprintsByProjectId(Guid projectId)
        {
            return await  _context.Sprints
                .Where(s=> s.ProjectId == projectId).ToListAsync();
        }



    }
}
