using System.ComponentModel.DataAnnotations.Schema;

public class FileEntity
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required byte[] Content { get; set; }
    public required DateTime DateTime { get; set; }
    public required string UserId { get; set; }
    [ForeignKey("UserId")]
    public required UserEntity User { get; set; }
    public required int FolderId { get; set; }
    [ForeignKey("FolderId")]
    public required Folder Folder { get; set; }


    
}