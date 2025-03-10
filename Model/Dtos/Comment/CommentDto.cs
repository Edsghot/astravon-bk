namespace Astravon.Model.Dtos.Comment;

public record CommentDto
{
    public int Id { get; set; }
    public int PostId { get; set; }
    public int UserId { get; set; }
    public string Content { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Mail { get; set; } = string.Empty;
    public DateTime DateComment { get; set; }
}