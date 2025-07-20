using OptiPlanBackend.Dto;
using OptiPlanBackend.Models;
using OptiPlanBackend.Repositories.Interfaces.OptiPlanBackend.Repositories.Interfaces;
using OptiPlanBackend.Services.Interfaces;


namespace OptiPlanBackend.Services.Implementations
{
    public class TaskService : ITaskService
    {

        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }


        public async Task<bool> CreateAsync(OptiPlanBackend.Models.ProjectTask task)
        {
            await _taskRepository.AddAsync(task);
            return await _taskRepository.SaveChangesAsync();

        }

        public Task<bool> DeleteAsync(ProjectTask project)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProjectTask>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ProjectTask?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProjectTask>> GetByOwnerIdAsync(Guid ownerId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProjectTaskCountDto>> GetMonthlyTasksByProjectAsync(Guid userId, int month, int year)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(ProjectTask task)
        {
            throw new NotImplementedException();
        }

        async Task<ProjectTask?> ITaskService.GetByIdAsync(Guid id)
        {
            return await _taskRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<IGrouping<(Guid ProjectId, string ProjectTitle), ProjectTask>>>
           GetUserTasksGroupedByProjectForMonth(Guid userId, int month, int year)
        {

            return await _taskRepository.GetUserTasksGroupedByProjectForMonth(userId, month, year);
        }

    }
}
