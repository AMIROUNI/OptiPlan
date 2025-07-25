using Microsoft.VisualBasic;
using OptiPlanBackend.Dtos;
using OptiPlanBackend.Models;
using OptiPlanBackend.Repositories.Implementations;
using OptiPlanBackend.Repositories.Interfaces;
using OptiPlanBackend.Services.Interfaces;

namespace OptiPlanBackend.Services.Implementations
{
    public class SprintService : ISprintService
    {
        private readonly ISprintRepository _sprintRepository;

        public SprintService(ISprintRepository sprintRepository)
        {
            _sprintRepository = sprintRepository;
        }

        public async Task<bool> CreateAsync(Sprint sprint)
        {
            await _sprintRepository.AddAsync(sprint);
            return await _sprintRepository.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Sprint sprint)
        {
            _sprintRepository.Delete(sprint);
            return await _sprintRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<Sprint>> GetAllAsync()
        {
            return await _sprintRepository.GetAllAsync();
        }

        public async Task<Sprint?> GetByIdAsync(Guid id)
        {
            return await _sprintRepository.GetByIdAsync(id);
        }

        public Task<IEnumerable<Sprint>> GetByOwnerIdAsync(Guid ownerId)
        {
            throw new NotImplementedException("Sprints do not have an OwnerId.");
        }

        public async Task<bool> UpdateAsync(Sprint sprint)
        {
            _sprintRepository.Update(sprint);
            return await _sprintRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<Sprint>> GetByProjectIdAsync(Guid projectId)
        {
            return await _sprintRepository.GetSprintsByProjectId(projectId);
        }

        public async Task<IEnumerable<Sprint>> GetSprintsByProjectId(Guid projectId)
        {
            return await _sprintRepository.GetSprintsByProjectId(projectId) ;
        }

        public  async Task<Sprint> CreateSprintForProject(Guid projectId, SprintDto sprintDto)
        {
            return await _sprintRepository.CreateSprintForProject(projectId, sprintDto);
        }
    }
}
