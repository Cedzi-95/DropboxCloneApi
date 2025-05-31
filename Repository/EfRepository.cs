
using Microsoft.EntityFrameworkCore;

public class EfRepository<T> : IRepository<T> where T : class
{
    public readonly AppDbContext _context;

    public EfRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task CreateAsync(T entity)
    {
         await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
      var entity =  await GetByIdAsync(id);
       if (entity != null)
            {
                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync();
            }
    }

    public async Task DeleteAsync(T entityToRemove)
    {
         _context.Set<T>().Remove(entityToRemove);
         await _context.SaveChangesAsync();
    }

    public async Task EditAsync(T updatedEntity)
    {
         _context.Entry(updatedEntity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}