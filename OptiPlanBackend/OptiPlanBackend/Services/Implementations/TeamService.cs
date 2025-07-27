using Microsoft.VisualBasic;
using OptiPlanBackend.Models;
using OptiPlanBackend.Repositories.Implementations;
using OptiPlanBackend.Repositories.Interfaces;
using OptiPlanBackend.Services.Interfaces;

namespace OptiPlanBackend.Services.Implementations
{
    public class TeamService : ITeamService
    {

        public readonly  ITeamRepository _teamRepository;

        public TeamService(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public async Task<bool> CreateAsync(Team team)
        {
              await _teamRepository.AddAsync(team);
            return await _teamRepository.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Team team)
        {
            _teamRepository.Delete(team);
            return await _teamRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<Team>> GetAllAsync()
        {
            return await _teamRepository.GetAllAsync();
        }

        public async Task<Team?> GetByIdAsync(Guid id)
        {
            return await _teamRepository.GetByIdAsync(id);
        }

        public async Task<bool> UpdateAsync(Team team)
        {
             _teamRepository.Update(team);
            return await _teamRepository.SaveChangesAsync();
        }
    }
}
