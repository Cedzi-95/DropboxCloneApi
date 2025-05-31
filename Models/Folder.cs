using System.ComponentModel.DataAnnotations.Schema;

public class Folder
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public ICollection<FileEntity> Files { get; set; } = new List<FileEntity>();
    public required Guid UserId { get; set; }
    [ForeignKey("UserId")]
    public UserEntity? CreatedBy { get; set; }

    
}