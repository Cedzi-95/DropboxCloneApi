using System.ComponentModel.DataAnnotations.Schema;

public class Folder
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required DateTime CreatedAt { get; set; }
    public ICollection<FileEntity> Files { get; set; } = new List<FileEntity>();
    public required Guid UserId { get; set; }
    [ForeignKey("UserId")]
    public UserEntity? CreatedBy { get; set; }


}

public class CreateFolderDto
{
    public required string Name { get; set; }
    // public required Guid UserId { get; set; }

}

public class UpdateFolderDto
{
    public string? Name { get; set; }
    // public  Guid UserId { get; set; }

}


public class FolderResponse
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int FileCount { get; set; }
    public required Guid UserId { get; set; }
    public required string CreatedByUsername { get; set; } 
    public DateTime CreatedAt { get; set; }

}