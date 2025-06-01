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
   // [Authorize]
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
            return CreatedAtAction(nameof(CreateFileAsync), new { id = fileEntity.Id }, response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating file");
            return StatusCode(500, "An error occurred while creating the file");
        }
    }
}