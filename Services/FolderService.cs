
using Microsoft.EntityFrameworkCore;

public class FolderService : IFolderService
{
    private readonly IRepository<Folder> _folderRepository;
    private readonly IUserService _userService;
    private readonly IFileService _fileService;

    public FolderService(IRepository<Folder> folderRepository,
     IUserService userService,
      IFileService fileService)
    {
        _folderRepository = folderRepository;
        _userService = userService;
        _fileService = fileService;
    }
    public Task<bool> CanDeleteFolderAsync(int folderId, Guid userId)
    {
        throw new NotImplementedException();
    }

    public async Task<Folder> CreateFolderAsync(Guid userId, CreateFolderDto createFolderDto)
    {
        var user = await _userService.GetUserByIdAsync(userId.ToString());
        if (user == null)
        {
            throw new UnauthorizedAccessException("User not found");
        }
        var folder = new Folder
        {
            Id = 0,
            Name = createFolderDto.Name,
            CreatedAt = DateTime.UtcNow,
            UserId = userId,
            CreatedBy = user
        };
        
        await _folderRepository.CreateAsync(folder);

        return folder;

    }

    public Task<Folder> DeleteFolderAsync(int folderId, Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<Folder?> GetFolderByIdAsync(int folderId, Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<int> GetFolderFileCountAsync(int folderId, Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<long> GetFolderTotalSizeAsync(int folderId, Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<Folder> GetFolderWithFilesAsync(int folderId, Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Folder>> GetUserFoldersAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsFolderNameUniqueAsync(string name, Guid userId, int? excludeFolderId = null)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Folder>> SearchFoldersAsync(string searchTerm, Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<Folder> UpdateFolderAsync(Guid userId, UpdateFolderDto updateFolder, int folderId)
    {
        throw new NotImplementedException();
    }
}