using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class AppDbContext(DbContextOptions<AppDbContext> options)
: IdentityDbContext<UserEntity, IdentityRole<Guid>, Guid>(options)
{
    public DbSet<FileEntity> Files {get; set; }
    public DbSet<Folder> Folders { get; set; }

   
}