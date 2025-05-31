
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

namespace DropboxCloneApi;

public class Program
{
    public async static Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddIdentity<UserEntity, IdentityRole<Guid>>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 6;
        })
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

        builder.Services.AddScoped<IUserService, UserService>();

         

        // Add services to the container.
        builder.Services.AddAuthorization();

         builder.Services.AddCors(options =>
        {
            options.AddPolicy(
                "AllowAll",
                builder =>
                {
                    builder
                        .WithOrigins("http://localhost:5173")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                }
            );
        });

       
        var app = builder.Build();

        app.MapControllers();

         await CreateDefaultRoles(app);
        await CreateAdminAccount(app);

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.MapScalarApiReference();
        }
        app.UseAuthentication();
        app.UseAuthorization();



       

        app.Run();
    }

     static async Task CreateDefaultRoles(WebApplication app)
    {
        using var scope = app.Services.CreateAsyncScope();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

        if (await roleManager.FindByNameAsync("Admin") == null)
        {
            await roleManager.CreateAsync(new IdentityRole<Guid>("Admin"));
        }
        if (await roleManager.FindByNameAsync("User") == null)
        {
            await roleManager.CreateAsync(new IdentityRole<Guid>("User"));
        }
    }

    static async Task CreateAdminAccount(WebApplication app)
    {
        using var scope = app.Services.CreateAsyncScope();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserEntity>>();

        if (await userManager.FindByNameAsync("Admin") == null)
        {
            var user = new UserEntity { UserName = "Admin" };
            var password = "Pass123!";
            var createUserResult = await userManager.CreateAsync(user, password);
            if (!createUserResult.Succeeded)
            {
                throw new Exception("Unable to create Admin user.");
            }
            var addRoleResult = await userManager.AddToRoleAsync(user, "Admin");
            if (!addRoleResult.Succeeded)
            {
                throw new Exception("Unable to assign role: 'Admin' to Admin user.");
            }
        }
    }
}
