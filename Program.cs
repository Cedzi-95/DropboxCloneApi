
namespace DropboxCloneApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();

        // Add services to the container.
        builder.Services.AddAuthorization();

       
        var app = builder.Build();
        app.MapControllers();

       

        app.Run();
    }
}
