
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Scalar.AspNetCore;

namespace DropboxCloneApi;

public class Program
{
    public  static void  Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

         builder.Services.AddIdentityApiEndpoints<UserEntity>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        builder.Services.Configure<IdentityOptions>(Options =>
        {
            Options.Password.RequireDigit = true;
            Options.Password.RequiredLength = 6;
        });


        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IFolderService, FolderService>();
        builder.Services.AddScoped<IFileService, FileService>();
        builder.Services.AddScoped<IFileRepository, FileRepository>();
        builder.Services.AddScoped<IRepository<FileEntity>>(sp => sp.GetRequiredService<IFileRepository>());

        builder.Services.AddScoped<IRepository<Folder>, FolderRepository>();



        builder.Services.AddAuthorization();

        var app = builder.Build();

        app.MapControllers();

        app.MapIdentityApi<UserEntity>();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.MapScalarApiReference();
        }
        app.UseAuthentication();
        app.UseAuthorization();





        app.Run();
    }

}
