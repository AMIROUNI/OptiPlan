using OptiPlanBackend.Dto;
using OptiPlanBackend.Models;





namespace OptiPlanBackend.Services.Interfaces
    {
        public interface ITaskService
        {

         Task<IEnumerable<OptiPlanBackend.Models.ProjectTask>> GetAllAsync();
        Task<ProjectTask?> GetByIdAsync(Guid id);
        Task<IEnumerable<OptiPlanBackend.Models.ProjectTask>> GetByOwnerIdAsync(Guid ownerId);
        Task<bool> CreateAsync(OptiPlanBackend.Models.ProjectTask project);
        Task<bool> UpdateAsync(OptiPlanBackend.Models.ProjectTask project);
        Task<bool> DeleteAsync(OptiPlanBackend.Models.ProjectTask project);

        public  Task<IEnumerable<IGrouping<(Guid ProjectId, string ProjectTitle), ProjectTask>>>
           GetUserTasksGroupedByProjectForMonth(Guid userId, int month, int year);

        public Task<IEnumerable<ProjectTask>> GetProjectTasksByProjectIdAsync(Guid projectId);

        public  Task<ProjectTask> AddProjectTaskForAProject(Dto.ProjectTaskDto projectTaskDto,Guid userId);
    }
    }


