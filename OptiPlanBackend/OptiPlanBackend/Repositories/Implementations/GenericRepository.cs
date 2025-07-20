using Microsoft.EntityFrameworkCore;
using OptiPlanBackend.Data;
using OptiPlanBackend.Repositories.Interfaces;
using System.Linq.Expressions;

namespace OptiPlanBackend.Repositories.Implementations
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly UserDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(UserDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

        public async Task<T?> GetByIdAsync(Guid id) => await _dbSet.FindAsync(id);

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate) =>
            await _dbSet.Where(predicate).ToListAsync();

        public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

        public void Update(T entity) => _dbSet.Update(entity);

        public void Delete(T entity) => _dbSet.Remove(entity);

        public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;
    }

}
