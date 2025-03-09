namespace Astravon.Model.Dtos.Post;

public class CreatePostDto
{
    public int? UserId { get; set; }
    public string? Content { get; set; }
    public string? PostUrl { get; set; }
    public IFormFile? MediaFile { get; set; }
}