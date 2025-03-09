namespace Astravon.Model.Dtos.Comment;

public record CreateCommentDto
{
    public int? PostId { get; set; }
    public int? UserId { get; set; }
    public string? Content { get; set; } 
}