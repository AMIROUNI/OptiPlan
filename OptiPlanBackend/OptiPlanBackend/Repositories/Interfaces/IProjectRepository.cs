﻿
using OptiPlanBackend.Models;
namespace OptiPlanBackend.Repositories.Interfaces
{
    public interface IProjectRepository : IGenericRepository<Project>
    {
        Task<IEnumerable<Project>> GetByOwnerIdAsync(Guid ownerId);
        public  Task<IEnumerable<Project>> GetProjectsForUserAsync(Guid userId);

        public Task<Team> GetTeamByProjectId(Guid id);
        public  Task<IEnumerable<TeamMembership>> GetTeamMembershipsByProjectIdAsync(Guid projectId);
    }
}
