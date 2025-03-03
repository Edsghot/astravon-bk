namespace Astravon.Model.Dtos.project;

public record CreateResearchProjectDto
{
    public int? Id { get; set; }
    public string Title { get; set; }
    public string? Authors { get; set; }
    public string? Year { get; set; }
    public string? Description { get; set; }
    public string? Summary { get; set; }
    public DateTime? Date { get; set; }
    public string? Doi { get; set; }
    public IFormFile? File { get; set; }
    public string? Editor { get; set; }
    public int IdTeacher { get; set; }
}