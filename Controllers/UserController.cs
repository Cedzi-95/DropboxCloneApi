using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("user")]
public class UserController : ControllerBase
{
    private IUserService userService;

    public UserController(IUserService userService)
    {
        this.userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
    {
        var user = await userService.RegisterUserAsync(request);
        return Ok();
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