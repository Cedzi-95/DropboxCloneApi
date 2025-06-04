using Microsoft.EntityFrameworkCore;
public interface IFileRepository : IRepository<FileEntity>
{

    public  Task<IEnumerable<FileEntity>> GetFilesByUserIdAsync(Guid userId);
    public  Task<IEnumerable<FileEntity>> GetFilesByFolderIdAsync(int folderId);
    public  Task<IEnumerable<FileEntity>> GetFilesByUserAndFolderAsync(Guid userId, int folderId);
    public  Task<IEnumerable<FileEntity>> SearchFilesByNameAsync(string searchTerm, Guid userId);
     public  Task<long> GetTotalFileSizeByUserAsync(Guid userId);
     public Task<byte[]> GetFileContentAsync(int fileId);
     public Task<string> GetFileNameAsync();

}

public class FileRepository : EfRepository<FileEntity>, IFileRepository
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

    public async Task<byte[]> GetFileContentAsync(int fileId)
    {
       return await _context.Set<FileEntity>()
       .Where(f => f.Id == fileId)
       .Select(f => f.Content)
       .FirstOrDefaultAsync() ?? throw new FileNotFoundException($"File with ID {fileId} not found.");
       
    }

    public async Task<string> GetFileNameAsync()
    {
       return await _context.Set<FileEntity>()
       .Select(f => f.Name)
       .FirstOrDefaultAsync() ?? throw new FileNotFoundException("File not found");
    }
}