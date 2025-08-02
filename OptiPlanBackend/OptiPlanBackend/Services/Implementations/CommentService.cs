using OptiPlanBackend.Models;
using OptiPlanBackend.Services.Interfaces;
using OptiPlanBackend.Repositories.Interfaces;
using OptiPlanBackend.Data;
using Microsoft.EntityFrameworkCore;

namespace OptiPlanBackend.Services.Implementations
{
    public class CommentService : ICommentService
    {
        private readonly  ICommentRepository _commentRepository;
        private readonly UserDbContext _context;

        public CommentService(ICommentRepository commentRepository, UserDbContext context)
        {
            _commentRepository = commentRepository;
            _context = context;
        }

        public async Task<bool> CreateAsync(Comment comment)
        {
            await _commentRepository.AddAsync(comment);
            return await _commentRepository.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Comment comment)
        {
            _commentRepository.Delete(comment);
            return await _commentRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<Comment>> GetAllAsync()
        {
            return await _commentRepository.GetAllAsync();
        }

        public async Task<Comment?> GetByIdAsync(Guid id)
        {
            return await _commentRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Comment>> GetByUserIdAsync(Guid userId)
        {
            return await _commentRepository.FindAsync(c => c.AuthorId == userId);
        }

        public async Task<IEnumerable<Comment>> GetCommentsByWorkItemIdAsync(Guid workItemId)
        {
            return await _commentRepository.GetCommentsByWorkItemIdAsync(workItemId);
        }

        public async Task<Team> GetTeamWithProjectAsync(Guid teamId)
        {
            return await _context.Teams
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.Id == teamId);
        }

        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<bool> UpdateAsync(Comment comment)
        {
            _commentRepository.Update(comment);
            return await _commentRepository.SaveChangesAsync();
        }
    }
}
