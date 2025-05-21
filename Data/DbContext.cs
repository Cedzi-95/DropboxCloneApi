using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : IdentityDbContext<UserEntity>
{
    public DbSet<FileEntity> Files {get; set; }
    public DbSet<Folder> Folders { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base (options)
    {
        
    }
}