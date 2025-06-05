using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("folder")]
public class FolderController : ControllerBase
{
    private readonly IFolderService _folderService;
    private readonly ILogger<FolderController> _logger;

    public FolderController(IFolderService folderService, ILogger<FolderController> logger)
    {
        _folderService = folderService;
        _logger = logger;
    }
    
    [Authorize]
    [HttpPost("create")]
    public async Task<IActionResult> CreateFolderAsync([FromBody] CreateFolderDto createFolderDto)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdClaim, out Guid userId))
            {
                return Unauthorized("Invalid user token");
            }

            var folder = await _folderService.CreateFolderAsync(userId, createFolderDto);

            // Map to response DTO
            var response = new FolderResponse
            {
                Id = folder.Id,
                Name = folder.Name,
                FileCount = folder.Files?.Count ?? 0,
                UserId = folder.UserId,
                CreatedByUsername = folder.CreatedBy?.UserName ?? "Unknown", // Adjust property name as needed
                CreatedAt = folder.CreatedAt
            };

            return Created($"/folder/{folder.Id}", response);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating folder");
            return StatusCode(500, "Internal server error");
        }
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdClaim, out Guid userId))
            {
                return Unauthorized("Invalid user token");
            }
            var folder = await _folderService.GetFolderByIdAsync(id, userId);
            if (folder == null)
            {
                return NotFound("Folder not found");
            }

            var deletedFolder = await _folderService.DeleteFolderAsync(id, folder.UserId);
            return Ok(new { message = "Folder deleted successfully!", folder = deletedFolder });
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

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetFoldersById(int id)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdClaim, out Guid userId))
            {
                return Unauthorized("Invalid user token");
            }
            var folder = await _folderService.GetFolderByIdAsync(id, userId);
            if (folder == null)
            {
                return NotFound("Folder not found");
            }
            return Ok(folder);
        }
        catch (Exception ex)
        {
            throw new ArgumentException(ex.Message);
        }

    }
    [Authorize]
     [HttpGet("all")]
    public async Task<IActionResult> getAllFoldersAsync()
    {
       var folders = await _folderService.GetAllFoldersAsync();
        return Ok(folders);
    }
}