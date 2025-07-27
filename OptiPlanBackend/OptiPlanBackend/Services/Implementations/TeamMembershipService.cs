using Microsoft.VisualBasic;
using OptiPlanBackend.Models;
using OptiPlanBackend.Repositories.Interfaces;
using OptiPlanBackend.Services.Interfaces;

namespace OptiPlanBackend.Services.Implementations
{
    public class TeamMembershipService : ITeamMembershipService
    {

        private readonly ITeamMembershipRepository _teamMembershipRepository;
        public TeamMembershipService(ITeamMembershipRepository teamMembershipRepository)
        {
            _teamMembershipRepository = teamMembershipRepository;
        }

        public async Task<bool> CreateAsync(TeamMembership team)
        {
            await _teamMembershipRepository.AddAsync(team);
            return await _teamMembershipRepository.SaveChangesAsync();


        }

        public Task<bool> DeleteAsync(TeamMembership team)
        {
            _teamMembershipRepository.Delete(team);
            return _teamMembershipRepository.SaveChangesAsync();
        }

        public Task<IEnumerable<TeamMembership>> GetAllAsync()
        {
            return _teamMembershipRepository.GetAllAsync();
        }

        public async Task<TeamMembership?> GetByIdAsync(Guid id)
        {
           return await _teamMembershipRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<TeamMembership>> GetByProjectIdAsync(Guid projectId)
        {
           return await _teamMembershipRepository.GetByProjectIdAsync(projectId);
        }

        public async Task<TeamMembership?> GetByUserAndProjectIdAsync(Guid userId, Guid projectId)
        {
           return await _teamMembershipRepository.GetByUserAndProjectIdAsync(userId, projectId);
        }

        public async Task<IEnumerable<TeamMembership>> GetByUserIdAsync(Guid userId)
        { 
             return await _teamMembershipRepository.GetByUserIdAsync(userId);
        }

        public async Task<bool> UpdateAsync(TeamMembership team)
        {
            _teamMembershipRepository.Update(team);
            return await _teamMembershipRepository.SaveChangesAsync();
        }
    }
}
