using Microsoft.EntityFrameworkCore;

public class FileRepository : EfRepository<FileEntity>
{
    public FileRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<FileEntity>> GetFilesByUserIdAsync(Guid userId)
    {
        return await _context.Set<FileEntity>()
        .Include(f => f.Folder)
        .Include(f => f.User)
        .Where(f => f.UserId == userId)
        .ToListAsync();
    }

//form a certain folder
    public async Task<IEnumerable<FileEntity>> GetFilesByFolderIdAsync(int folderId)
    {
        return await _context.Set<FileEntity>()
            .Include(f => f.User)
            .Where(f => f.FolderId == folderId)
            .ToListAsync();
    }
//from a certain user and folder
    public async Task<IEnumerable<FileEntity>> GetFilesByUserAndFolderAsync(Guid userId, int folderId)
    {
        return await _context.Set<FileEntity>()
            .Include(f => f.Folder)
            .Include(f => f.User)
            .Where(f => f.UserId == userId && f.FolderId == folderId)
            .ToListAsync();
    }
    public async Task<IEnumerable<FileEntity>> SearchFilesByNameAsync(string searchTerm, Guid userId)
    {
        return await _context.Set<FileEntity>()
            .Include(f => f.Folder)
            .Include(f => f.User)
            .Where(f => f.UserId == userId && f.Name.Contains(searchTerm))
            .ToListAsync();
    }

     public async Task<long> GetTotalFileSizeByUserAsync(Guid userId)
    {
        return await _context.Set<FileEntity>()
            .Where(f => f.UserId == userId)
            .SumAsync(f => f.Content.Length);
    }

    
}