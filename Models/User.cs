using Microsoft.AspNetCore.Identity;

public class UserEntity : IdentityUser
{
    ICollection<Folder> Folders { get; set; }
    ICollection<FileEntity> Files { get; set; } 

    public UserEntity()
    {
        Folders = [];
        Files = [];
    }
    public UserEntity( string username, string email, string password) : base(username)
    {
        
    }

}