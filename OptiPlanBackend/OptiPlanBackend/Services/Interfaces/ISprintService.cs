using OptiPlanBackend.Dto;
using OptiPlanBackend.Dtos;
using OptiPlanBackend.Models;

namespace OptiPlanBackend.Services.Interfaces
{
    public interface ISprintService
    {
        Task<IEnumerable<Sprint>> GetAllAsync();
        Task<Sprint?> GetByIdAsync(Guid id);
        Task<IEnumerable<Sprint>> GetByOwnerIdAsync(Guid ownerId);
        Task<bool> CreateAsync(Sprint project);
        Task<bool> UpdateAsync(Sprint project);
        Task<bool> DeleteAsync(Sprint project);


        public Task<IEnumerable<Sprint>> GetSprintsByProjectId(Guid projectId);
        public Task<Sprint> CreateSprintForProject(Guid projectId, SprintDto sprintDto);

    }
}
