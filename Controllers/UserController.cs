using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("user")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UserController> _logger;

    public UserController(IUserService userService, ILogger<UserController> logger)
    {
        this._userService = userService;
        this._logger = logger;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
    {
       try
        {
            var result = await _userService.RegisterUserAsync(request);
            return Ok(new { UserId = result.Id, Message = "Registration successful" });
        }
        catch (IdentityException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during user registration");
            return StatusCode(500, new { Message = "An unexpected error occurred. Please try again later." });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] SignInUserRequest request)
    {
        try
        {
            var result = await _userService.LoginUserAsync(request);
            return Ok(result);
        }
        catch (ArgumentException)
        {
            return BadRequest( new { errors = "User not found"});
        }

        catch (IdentityException ex)
        {
            return Unauthorized(new { errors = ex.Message });
        }
        catch
        {
            return StatusCode(500, new { errors = "An unexpected error occured." });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsersAsync()
    {
        var result = await _userService.GetAllAsync();
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserByIdAsync(string id)
    {
        var result = await _userService.GetUserByIdAsync(id);
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }

     [HttpDelete("{id}")]
    //[Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            await _userService.DeleteAsync(id);

            return NoContent();
        }
        catch
        {
            return BadRequest($"Unable to delete user with id: {id}");
        }
    }

}



public class SignInUserRequest
{
    public required string Username {get; set;}
    public required string Password { get; set; }
}

public class SignInUserResponse 
{
    public required Guid Id { get; set; }
    public required string Username { get; set; }
}

public class RegisterUserRequest 
{
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}

public class RegisterUserResponse 
{
    public Guid Id { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public DateTime CreatedAt { get; set; }
}
public class EditUserRequest
{
    public required Guid Id { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
}
