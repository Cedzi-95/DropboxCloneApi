using Microsoft.AspNetCore.Identity;

public class UserEntity : IdentityUser<Guid>
{
   public ICollection<Folder> Folders { get; set; }
    public ICollection<FileEntity> Files { get; set; } 

    public UserEntity()
    {
        Folders = [];
        Files = [];
    }
    public UserEntity( string username, string email) : base(username)
    {
        Email = email;
         Folders = [];
        Files = [];
    }

}