
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DropboxCloneApi;

public class Program
{
    public static void Main(string[] args)
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

       
        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

       

        app.Run();
    }
}
