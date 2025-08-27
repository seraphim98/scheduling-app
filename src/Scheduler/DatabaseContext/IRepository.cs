namespace Scheduler.Database;

public interface IRepository<T>
    where T : class
{
    public Task<List<T>> Query();
    public Task<T?> GetByIdAsync(Guid id);
    public Task CreateAsync(T entity);
    public Task UpdateAsync(T entity);
    public Task DeleteAsync(T entity);
    public Task SaveAsync();
    public void Create(T entity);
    public void Update(T entity);
    public void Delete(T entity);
}
