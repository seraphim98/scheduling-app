using Microsoft.EntityFrameworkCore;

namespace Scheduler.Database;

public class Repository<T>(DatabaseContext context) : IRepository<T>
    where T : class
{
    DatabaseContext _context = context;
    DbSet<T> _dbSet = context.Set<T>();

    public async Task<List<T>> Query()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task CreateAsync(T entity)
    {
        _dbSet.Add(entity);
        await SaveAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await SaveAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        await SaveAsync();
    }

    public void Create(T entity)
    {
        _dbSet.Add(entity);
    }

    public void Update(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}
