using Microsoft.AspNetCore.Identity;

public class UserEntity : IdentityUser<Guid>
{
   public ICollection<Folder> Folders { get; set; } = new List<Folder>();
    public ICollection<FileEntity> Files { get; set; } = new List<FileEntity>();

    public UserEntity()
    {
    }
    public UserEntity( string username, string email) : base(username)
    {
        Email = email;
         
    }

}