using System.ComponentModel.DataAnnotations.Schema;

public class Folder
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    ICollection<FileEntity> Files { get; set; } = new List<FileEntity>();
    public required string UserId { get; set; }
    [ForeignKey("UserId")]
    public required UserEntity User { get; set; }
    
}