
using Microsoft.AspNetCore.Http.HttpResults;

public class FileService : IFileService
{
    //  private readonly IRepository<FileEntity> _fileRepository; 
    private readonly IFileRepository _fileRepository;
    private readonly IUserService _userService;
    private readonly IRepository<Folder> _folderRepository;

    public FileService(IFileRepository fileRepository,
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
            var contentBytes = Convert.FromBase64String(uploadFile.ByteContent);
        }
        catch (FormatException)
        {
            throw new ArgumentException("Invalid Base64 content");
        }

        var fileEntity = new FileEntity
        {
            Name = uploadFile.Name,
            Content = Convert.FromBase64String(uploadFile.ByteContent), // Convert back to byte[]
            ContentType = uploadFile.ContentType,
            Size = uploadFile.ByteContent.Length,
            CreatedAt = DateTime.UtcNow,
            UserId = user.Id,
            FolderId = uploadFile.FolderId
        };
        await _fileRepository.CreateAsync(fileEntity);
        return fileEntity;

    }

    public async Task<FileEntity> DeleteFileAsync(int fileId, Guid userId)
    {
        var file = await _fileRepository.GetByIdAsync(fileId);
        if (file == null)
        {
            throw new ArgumentException("File not found");
        }
        var user = await _userService.GetUserByIdAsync(userId.ToString());
        if (user == null)
        {
            throw new UnauthorizedAccessException("User not found or there is no access to the file");
        }
        await _fileRepository.DeleteAsync(file);
        return file;
    }

    public Task<FileEntity> EditFileAsync(int fileId, UpdateFileDto editFile, Guid userId)
    {
        throw new NotImplementedException();
    }

    public async Task<byte[]> GetFileContentAsync(int fileId, Guid userId)
    {
       var content = await _fileRepository.GetFileContentAsync(fileId);
       return content;
    }

    public  Task<IEnumerable<FileEntity>> GetFilesByContentTypeAsync(string contentType, Guid userId)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<FileEntity>> GetFilesByFolderAsync(int folderId, Guid userId)
    {
        var files = await _fileRepository.GetFilesByFolderIdAsync(folderId);
        if (files == null)
        {
            throw new ArgumentException("No files were found.");
        }
        return files;
    }

    public async Task<FileEntity?> GetFileByIdAsync(int fileId)
    {
        return await _fileRepository.GetByIdAsync(fileId);
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

    public async Task<string> GetFileNameAsync(int fileId, Guid userId)
    {
       return await _fileRepository.GetFileNameAsync();
    }
    
    
}