namespace Astragon.Modules.Research.Domain.Entity;

public record ResearchProjectEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Authors { get; set; } = string.Empty;
    public string Pdf { get; set; } = string.Empty;
    public string Year { get; set; } = string.Empty;
    public int IdTeacher { get; set; }
}