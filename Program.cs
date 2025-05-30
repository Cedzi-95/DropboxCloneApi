
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
            options.UseNpgsql(DbConfig.connectionString)
        );
    builder.Services.AddIdentityCore<UserEntity>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddApiEndpoints();

        // Add services to the container.
        builder.Services.AddAuthorization();

       
        var app = builder.Build();
        app.MapControllers();

       

        app.Run();
    }
}
