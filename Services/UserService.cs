using System.Security.Principal;
using Microsoft.AspNetCore.Identity;

public interface IUserService
{
    public Task<UserEntity?> GetUserByIdAsync(string UserId);
    public Task<UserEntity> RegisterUserAsync(RegisterUserRequest request);
    public Task<UserEntity> LoginUser(SignInUserRequest request);
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

    public async Task<UserEntity> LoginUser(SignInUserRequest request)
    {
        var loggedInUser = await userManager.FindByNameAsync(request.Username) ??
        throw new IdentityException("Invalid Username");

    }

    public async Task<UserEntity> RegisterUserAsync(RegisterUserRequest request)
    {
        var user = new UserEntity(request.Username, request.Email);
        var result = await userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            throw new IdentityException(result);
        }

        await userManager.AddToRoleAsync(user, "user");
        return user;
    }

}
