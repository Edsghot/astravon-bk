namespace Astravon.Model.Dtos.Post;

public record UpdateCommentDto
{
    public int Id { get; set; }
    public string? Content { get; set; }
}


public record UpdatePostDto
{
    public int Id { get; set; }
    public string? Content { get; set; }
    public string? PostUrl { get; set; }
    public IFormFile? MediaFile { get; set; }
}



