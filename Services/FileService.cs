
public class FileService : IFileService
{
     private readonly IRepository<FileEntity> _fileRepository; 
    private readonly IUserService _userService;
    private readonly IRepository<Folder> _folderRepository;

    public FileService(IRepository<FileEntity> fileRepository,
     IUserService userService,
      IRepository<Folder> folderRepository)
    {
        _fileRepository = fileRepository;
        _userService = userService;
       _folderRepository = folderRepository;
    }

    public async Task<FileEntity> CreateFileAsync(Guid userId, CreateFileDto uploadFile)
    {
        var user = await _userService.GetUserByIdAsync(userId.ToString());
        if (user == null)
        {
            throw new UnauthorizedAccessException("User not found");
        }
        var folder = await _folderRepository.GetByIdAsync(uploadFile.FolderId);
        if (folder == null)
        {
            throw new ArgumentException("Folder not found or access denied");
        }
         // Validate Base64 content
        try
        {
            var contentBytes = Convert.FromBase64String(uploadFile.Content);
        }
        catch (FormatException)
        {
            throw new ArgumentException("Invalid Base64 content");
        }

        var fileEntity = new FileEntity
        {
            Name = uploadFile.Name,
            Content = Convert.FromBase64String(uploadFile.Content), // Convert back to byte[]
            ContentType = uploadFile.ContentType,
            Size = uploadFile.Content.Length,
            CreatedAt = DateTime.UtcNow,
            UserId = user.Id,
            FolderId = uploadFile.FolderId
        };
        await _fileRepository.CreateAsync(fileEntity);
        return fileEntity;

    }

    public Task<FileEntity> DeleteFileAsync(int fileId, Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<FileEntity> EditFileAsync(int fileId, UpdateFileDto editFile, Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<byte[]> GetFileContentAsync(int fileId, Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<FileEntity>> GetFilesByContentTypeAsync(string contentType, Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<FileEntity>> GetFilesByFolderAsync(int folderId, Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<FileEntity>> GetFilesByIdAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<long> GetUserTotalFileSizeAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task MoveFileToFolderAsync(int fileId, int newFolderId, Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<FileEntity>> SearchFilesAsync(string searchTerm, Guid userId)
    {
        throw new NotImplementedException();
    }
}