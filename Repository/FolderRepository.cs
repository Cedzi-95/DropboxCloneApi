using Microsoft.EntityFrameworkCore;
public interface IFolderRepository : IRepository<Folder>
{
     public  Task<IEnumerable<Folder>> GetFoldersByUserIdAsync(Guid userId);
     public  Task<Folder?> GetFolderWithFilesAsync(int folderId);
     public  Task<bool> UserOwnsFolderAsync(int folderId, Guid userId);
    public  Task<IEnumerable<Folder>> SearchFoldersByNameAsync(string searchTerm, Guid userId);
}

public class FolderRepository : EfRepository<Folder>, IFolderRepository
{
    public FolderRepository(AppDbContext context) : base(context)
    {
    }

     public async Task<IEnumerable<Folder>> GetFoldersByUserIdAsync(Guid userId)
    {
        return await _context.Set<Folder>()
            .Include(f => f.Files)
            .Include(f => f.CreatedBy)
            .Where(f => f.UserId == userId)
            .ToListAsync();
    }
//get folder with its files
     public async Task<Folder?> GetFolderWithFilesAsync(int folderId)
    {
        return await _context.Set<Folder>()
            .Include(f => f.Files)
            .Include(f => f.CreatedBy)
            .FirstOrDefaultAsync(f => f.Id == folderId);
    }

    //check if user owns the folder
     public async Task<bool> UserOwnsFolderAsync(int folderId, Guid userId)
    {
        return await _context.Set<Folder>()
            .AnyAsync(f => f.Id == folderId && f.UserId == userId);
    }
//search by name
    public async Task<IEnumerable<Folder>> SearchFoldersByNameAsync(string searchTerm, Guid userId)
    {
        return await _context.Set<Folder>()
            .Include(f => f.Files)
            .Where(f => f.UserId == userId && f.Name.Contains(searchTerm))
            .ToListAsync();
    }


}