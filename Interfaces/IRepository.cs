public interface IRepository<T>
{
    public abstract Task CreateAsync(T entity);
    public Task<T?> GetByIdAsync(int id);
    public Task<IEnumerable<T>> GetAllAsync();
    public Task EditAsync(T updatedEntity);
    public Task DeleteAsync(T entityToRemove);
    public Task SaveChangesAsync();
}