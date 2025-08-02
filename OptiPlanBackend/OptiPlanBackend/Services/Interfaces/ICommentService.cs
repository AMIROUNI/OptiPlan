using OptiPlanBackend.Models;

namespace OptiPlanBackend.Services.Interfaces
{
    public interface ICommentService
    {
        Task<IEnumerable<Comment>> GetAllAsync();
        Task<Comment?> GetByIdAsync(Guid id);
        Task<bool> CreateAsync(Comment invitation);
        Task<bool> UpdateAsync(Comment invitation);
        Task<bool> DeleteAsync(Comment invitation);
        Task<Team> GetTeamWithProjectAsync(Guid teamId);
        Task<User> GetUserByIdAsync(Guid userId);
        public Task<IEnumerable<Comment>> GetByUserIdAsync(Guid userId);

        public Task<IEnumerable<Comment>> GetCommentsByWorkItemIdAsync(Guid workItemId);
    }

    }
