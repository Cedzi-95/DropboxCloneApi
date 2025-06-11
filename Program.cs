using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

namespace DropboxCloneApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        
        // Use the newer OpenAPI approach (like your working app)
        builder.Services.AddOpenApi();  // Instead of AddSwaggerGen()

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddIdentityApiEndpoints<UserEntity>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        builder.Services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 6;
        });

        // Your scoped services...
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IFolderService, FolderService>();
        builder.Services.AddScoped<IFileService, FileService>();
        builder.Services.AddScoped<IFileRepository, FileRepository>();
        builder.Services.AddScoped<IRepository<FileEntity>>(sp => sp.GetRequiredService<IFileRepository>());
        builder.Services.AddScoped<IFolderRepository, FolderRepository>();
        builder.Services.AddScoped<IRepository<Folder>>(sp => sp.GetRequiredService<IFolderRepository>());

        builder.Services.AddAuthorization();

        var app = builder.Build();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        app.MapIdentityApi<UserEntity>();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();           // Instead of UseSwagger()
            app.MapScalarApiReference(); // No UseSwaggerUI() needed
        }

        app.Run();
    }
}