namespace Astravon.Model.Dtos.Likes;

public record LikeDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int PostId { get; set; }
}