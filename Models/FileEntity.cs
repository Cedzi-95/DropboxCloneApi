using System.ComponentModel.DataAnnotations.Schema;

public class FileEntity
{
    public  int Id { get; set; }
    public required string Name { get; set; }
    public required byte[] Content { get; set; }
    public required string ContentType { get; set; }
    public long Size { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required Guid UserId { get; set; }
    [ForeignKey("UserId")]
    public  UserEntity? User { get; set; }
    public required int FolderId { get; set; }
    [ForeignKey("FolderId")]
    public Folder? Folder { get; set; }
   
}


public class CreateFileDto
{
    public required string Name { get; set; }
    public required string Content { get; set; }
    public required string ContentType { get; set; }
    public required int FolderId { get; set; }
}

public class UpdateFileDto
{
     public string? Name { get; set; }
    public int? FolderId { get; set; }
}




public class CreateFileResponse
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public string? ContentType { get; set; }
    public long Size { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required Guid UserId { get; set; }
    public required string CreatedByUsername { get; set; }
    public int FolderId { get; set; }
    public string? DownloadUrl { get; set; }

}