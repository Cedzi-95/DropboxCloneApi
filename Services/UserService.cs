using System.Security.Principal;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public interface IUserService
{
    public Task<UserEntity?> GetUserByIdAsync(string UserId);
    public Task<RegisterUserResponse> RegisterUserAsync(RegisterUserRequest request);
    public Task<SignInUserResponse> LoginUserAsync(SignInUserRequest request);
    public  Task<IEnumerable<UserEntity>> GetAllAsync();
    public Task<UserEntity?> DeleteAsync(string entityId);
}

public class UserService : IUserService
{
    private readonly UserManager<UserEntity> userManager;
    private readonly SignInManager<UserEntity> signInManager;

    public UserService(UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
    }
    public async Task<UserEntity?> GetUserByIdAsync(string UserId)
    {
        var user = userManager.FindByIdAsync(UserId);
        return await user;
    }

    public async Task<IEnumerable<UserEntity>> GetAllAsync()
    {
        return await userManager.Users.ToListAsync();
    }

    
public async Task<SignInUserResponse> LoginUserAsync(SignInUserRequest request)
{
    // Change from FindByNameAsync to FindByEmailAsync
    var user = await userManager.FindByEmailAsync(request.Email)
        ?? throw new IdentityException("Invalid email");

    // Use email instead of username for sign-in
    var result = await signInManager.PasswordSignInAsync(
        user.UserName,  // Use the found user's username
        request.Password,
        false,
        false
    );

    if (result.Succeeded)
    {
        return new SignInUserResponse { Id = user.Id, Username = user.UserName! };
    }
    throw new IdentityException($"Invalid email or password");
}
    public async Task<RegisterUserResponse> RegisterUserAsync(RegisterUserRequest request)
    {
        if (await userManager.FindByNameAsync(request.Username) != null)
            throw new IdentityException("Username already exists");

        if (await userManager.FindByEmailAsync(request.Email) != null)
            throw new IdentityException("Email already registered");

        var user = new UserEntity()
        {
            UserName = request.Username,
            Email = request.Email,
        };
        var result = await userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            var errorMessages = string.Join("; ", result.Errors.Select(e => e.Description));
            throw new IdentityException($"Error while creating user: {errorMessages}");
        }
        return new RegisterUserResponse
        {
            Id = user.Id,
            Username = user.UserName,
            Email = user.Email,
        };

    }

    public async Task<UserEntity?> DeleteAsync(string entityId)
    {
        var user = await userManager.FindByIdAsync(entityId.ToString());
        if (user == null)
            return null;

        var result = await userManager.DeleteAsync(user);
        if (result.Succeeded)
            return null;

        throw new IdentityException($"Unable to delete {user.UserName}");
    }
}
