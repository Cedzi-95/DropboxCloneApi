public class Folder
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    ICollection<FileEntity> Files { get; set; } = new List<FileEntity>();
}