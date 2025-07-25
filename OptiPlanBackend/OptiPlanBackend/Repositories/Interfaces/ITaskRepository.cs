using OptiPlanBackend.Dto;
using OptiPlanBackend.Models;

namespace OptiPlanBackend.Repositories.Interfaces
{

    namespace OptiPlanBackend.Repositories.Interfaces
    {
        public interface ITaskRepository : IGenericRepository<ProjectTask>
        {
            Task<IEnumerable<IGrouping<(Guid ProjectId, string ProjectTitle), ProjectTask>>>
                GetUserTasksGroupedByProjectForMonth(Guid userId, int month, int year);



            public Task<IEnumerable<ProjectTask>> GetProjectTasksByProjectIdAsync(Guid projectId);



            public Task<ProjectTask> AddProjectTaskForAProject(ProjectTaskDto projectTaskDto, Guid userId);
        }

    }
}
