using OptiPlanBackend.Dto;
using OptiPlanBackend.Models;
using OptiPlanBackend.Repositories.Interfaces;
using OptiPlanBackend.Services.Interfaces;

namespace OptiPlanBackend.Services.Implementations
{
    public class ProjectService : IProjectService
    {
        private readonly  IProjectRepository _projectRepository;

        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<IEnumerable<Project>> GetAllAsync()
        {
            return await _projectRepository.GetAllAsync();
        }

        public async Task<Project?> GetByIdAsync(Guid id)
        {
            return await _projectRepository.GetByIdAsync(id);
        }

        // ✨ Specific method: Use repository + filtering logic
        public async Task<IEnumerable<Project>> GetByOwnerIdAsync(Guid ownerId)
        {
            return await _projectRepository.FindAsync(p => p.OwnerId == ownerId);
        }

        public async Task<bool> CreateAsync(Project project)
        {
            await _projectRepository.AddAsync(project);
            return await _projectRepository.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Project project)
        {
            _projectRepository.Update(project);
            return await _projectRepository.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Project project)
        {
            _projectRepository.Delete(project);
            return await _projectRepository.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task<Project> CreateProjectAsync(ProjectDto dto, Guid owenrId)
        {
            var project = new Project
            {
                Title = dto.Title,
                Description = dto.Description,
                EndDate = dto.EndDate,
                StartDate = dto.StartDate,
                OwnerId = owenrId
            };


            await _projectRepository.AddAsync(project);
            await _projectRepository.SaveChangesAsync();

            return project;
           
        }

        public async Task<IEnumerable<Project>> GetProjectsForUserAsync(Guid userId)
        {
            return await _projectRepository.GetProjectsForUserAsync(userId);
        }

        public async Task<Team> GetTeamByProjectId(Guid projectId)
        {
            return await _projectRepository.GetTeamByProjectId(projectId);
        }

        public async Task<IEnumerable<User>> GetUsersByProjectIdAsync(Guid projectId)
        {
            return await _projectRepository.GetUsersByProjectIdAsync(projectId);
        }
    }

}
