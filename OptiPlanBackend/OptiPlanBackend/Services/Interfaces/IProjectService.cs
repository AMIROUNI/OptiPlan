using OptiPlanBackend.Dto;
using OptiPlanBackend.Models;

namespace OptiPlanBackend.Services.Interfaces
{
    public interface IProjectService
    {
        Task<IEnumerable<Project>> GetAllAsync();
        Task<Project?> GetByIdAsync(Guid id);
        Task<IEnumerable<Project>> GetByOwnerIdAsync(Guid ownerId);
        Task<bool> CreateAsync(Project project);
        Task<bool> UpdateAsync(Project project);
        Task<bool> DeleteAsync(Project project);
        System.Threading.Tasks.Task<Project> CreateProjectAsync(ProjectDto projectDto, Guid value);
        public Task<IEnumerable<Project>> GetProjectsForUserAsync(Guid userId);
        public  Task<Team> GetTeamByProjectId(Guid projectId);
        public  Task<IEnumerable<TeamMembership>> GetTeamMembershipsByProjectIdAsync(Guid projectId);
    }

}
