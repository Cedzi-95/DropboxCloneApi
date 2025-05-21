using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("user")]
public class UserController : ControllerBase
{

}

public class SignInUserRequest
{
    public required string Username {get; set;}
    public required string Password { get; set; }
}

public class RegisterUserRequest 
{
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}