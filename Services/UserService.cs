using System.Security.Principal;
using Microsoft.AspNetCore.Identity;

public interface IUserService
{
    public Task<UserEntity?> GetUserByIdAsync(string UserId);
    public Task<UserEntity> RegisterUserAsync(RegisterUserRequest request);
    public Task<SignInUserResponse> LoginUser(SignInUserRequest request);
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

    public async Task<SignInUserResponse> LoginUser(SignInUserRequest request)
    {
         var user =
            await userManager.FindByNameAsync(request.Username)
            ?? throw new IdentityException("Invalid username");

        var result = await signInManager.PasswordSignInAsync(
            request.Username,
            request.Password,
            false,
            false
        );

        if (result.Succeeded)
        {
            return new SignInUserResponse { Id = user.Id, Username = user.UserName! };
        }
        throw new IdentityException($"Invalid username or password");

    }

    public async Task<UserEntity> RegisterUserAsync(RegisterUserRequest request)
    {
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
        return user;
    }

}
