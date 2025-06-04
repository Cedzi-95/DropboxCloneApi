using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Npgsql.Replication;

[ApiController]
[Route("file")]
public class FileController : ControllerBase
{
    private readonly IFileService _fileService;
    private readonly ILogger<FileController> _logger;

    public FileController(IFileService fileService, ILogger<FileController> logger)
    {
        _fileService = fileService;
        _logger = logger;
    }

    [HttpPost("create")]
     [Authorize]
    public async Task<IActionResult> CreateFileAsync([FromBody] CreateFileDto createFileDto)
    {
        try
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized("Invalid or missing user ID");
            }
            var fileEntity = await _fileService.CreateFileAsync(userId, createFileDto);
            _logger.LogInformation("Created file with id '{}' ", fileEntity.Id);

            var response = new CreateFileResponse
            {
                Id = fileEntity.Id,
                Name = fileEntity.Name,
                ContentType = fileEntity.ContentType,
                Size = fileEntity.Size,
                CreatedAt = fileEntity.CreatedAt,
                UserId = fileEntity.UserId,
                CreatedByUsername = User.Identity?.Name ?? "Unknown",
                FolderId = fileEntity.FolderId,
                DownloadUrl = $"/api/files/{fileEntity.Id}/download"

            };
            return Created($"/file/{fileEntity.Id}", response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating file");
            return StatusCode(500, "An error occurred while creating the file");
        }
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetFileByIdAsync(int id)
    {

        var file = await _fileService.GetFileByIdAsync(id);
        if (file == null)
        {
            return NotFound($"File with ID {id} not found");
        }
        return Ok(file);

    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (user == null || !Guid.TryParse(user, out var userId))
            {
                return Unauthorized("Invalid or missing user ID");
            }

            var file = await _fileService.GetFileByIdAsync(id);
            if (file == null)
            {
                return NotFound($"File with ID {id} not found");
            }
            var deletedFile = await _fileService.DeleteFileAsync(id, userId);
            return Ok(new { message = "File deleted successfully!", file = deletedFile });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting folder");
            return StatusCode(500, "An error occurred while deleting the folder");
        }

    }

    [HttpGet("folder/{folderId}")]
    public async Task<IActionResult> GetFilesByFolderId(int folderId)
    {
        var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (user == null || !Guid.TryParse(user, out var userId))
        {
            return Unauthorized("Invalid or missing user ID");
        }
        var files = await _fileService.GetFilesByFolderAsync(folderId, userId);
        var fileDtos = files.Select(f => new FileDto
        {
            Id = f.Id,
            Name = f.Name,
            ContentType = f.ContentType,
            Size = f.Size,
            CreatedAt = f.CreatedAt,
            UserId = f.UserId,
            FolderId = f.FolderId,
            DownloadUrl = $"/api/files/{f.Id}/download"
        }).ToList();
        return Ok(fileDtos);

    }

    [HttpGet("content/{fileId}")]
    public async Task<IActionResult> GetFileContentAsync(int fileId)
    {
        var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (user == null || !Guid.TryParse(user, out var userId))
        {
            return Unauthorized("Invalid or missing user Id");
        }
        var fileData = await _fileService.GetFileContentAsync(fileId, userId);
        return Ok($"File content: {fileData}");
    }


    [HttpGet("download/{fileId}")]
    public async Task<IActionResult> DownloadFileAsync(int fileId)
    {
        var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (user == null || !Guid.TryParse(user, out var userId))
        {
            return Unauthorized("Invalid or missing user Id");
        }
        var fileData = await _fileService.GetFileContentAsync(fileId, userId);
        var fileName = await _fileService.GetFileNameAsync(fileId, userId); 
        var contentType = GetContentType(fileName); 
        return File(fileData, contentType, fileName);
    }
    private string GetContentType(string fileName) //method to convert from byte content
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        return extension switch
        {
            ".pdf" => "application/pdf",
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".txt" => "text/plain",
            ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            _ => "application/octet-stream"
        };
    }
}