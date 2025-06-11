
using Microsoft.EntityFrameworkCore;

public class FolderService : IFolderService
{
    private readonly IFolderRepository _folderRepository;
    private readonly IUserService _userService;
    private readonly IFileService _fileService;

    public FolderService(IFolderRepository folderRepository,
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

    public async Task<Folder> DeleteFolderAsync(int folderId, Guid userId)
    {
        var user = await _userService.GetUserByIdAsync(userId.ToString());
        if (user == null)
        {
            throw new UnauthorizedAccessException("User not found");
        }
        var folder = await _folderRepository.GetByIdAsync(folderId);
        if (folder == null)
        {
            throw new ArgumentException("Folder not found");
        }
        await _folderRepository.DeleteAsync(folder);
        return folder;
    }

    public async Task<Folder?> GetFolderByIdAsync(int folderId, Guid userId)
    {
         var folder = await _folderRepository.GetByIdAsync(folderId);
         return folder;
    }

    public async Task<IEnumerable<Folder>> GetAllFoldersAsync()
    {
        return await _folderRepository.GetAllAsync();
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