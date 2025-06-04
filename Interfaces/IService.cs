using System.Net.NetworkInformation;
using System.Security.Cryptography.X509Certificates;

public interface IFileService
{
    public Task<FileEntity> CreateFileAsync(Guid userId, CreateFileDto uploadFile);
    public Task<FileEntity> EditFileAsync(int fileId, UpdateFileDto editFile, Guid userId);
    public Task<FileEntity> DeleteFileAsync(int fileId, Guid userId);
    public Task<FileEntity?> GetFileByIdAsync(int fileId);
    public Task<IEnumerable<FileEntity>> GetFilesByFolderAsync(int folderId, Guid userId);
    public Task<IEnumerable<FileEntity>> SearchFilesAsync(string searchTerm, Guid userId);
    public Task<byte[]> GetFileContentAsync(int fileId, Guid userId);// Get file content for download
    public Task<long> GetUserTotalFileSizeAsync(Guid userId);     // Get total storage used by user
    public Task MoveFileToFolderAsync(int fileId, int newFolderId, Guid userId);
    public Task<IEnumerable<FileEntity>> GetFilesByContentTypeAsync(string contentType, Guid userId);
    public Task<string> GetFileNameAsync(int fileId, Guid userId);





}

public interface IFolderService
{
    public Task<Folder> CreateFolderAsync(Guid userId, CreateFolderDto createFolder);
    public Task<Folder?> GetFolderByIdAsync(int folderId, Guid userId);
    public Task<IEnumerable<Folder>> GetUserFoldersAsync(Guid userId);
    public Task<Folder> UpdateFolderAsync(Guid userId, UpdateFolderDto updateFolder, int folderId);
    public Task<Folder> DeleteFolderAsync(int folderId, Guid userId);
    public Task<IEnumerable<Folder>> SearchFoldersAsync(string searchTerm, Guid userId);
    public Task<bool> IsFolderNameUniqueAsync(string name, Guid userId, int? excludeFolderId = null);
    public Task<int> GetFolderFileCountAsync(int folderId, Guid userId);
    public Task<long> GetFolderTotalSizeAsync(int folderId, Guid userId);
    public Task<bool> CanDeleteFolderAsync(int folderId, Guid userId);
    public Task<Folder> GetFolderWithFilesAsync(int folderId, Guid userId);
     public Task<IEnumerable<Folder>> GetAllFoldersAsync();
}