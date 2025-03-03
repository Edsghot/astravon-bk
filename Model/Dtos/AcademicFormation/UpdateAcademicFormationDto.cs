namespace Astravon.Model.Dtos.AcademicFormation;

public record UpdateAcademicFormationDto
{
    public int Id { get; set; }
    public string? Degree { get; set; } = string.Empty;
    public string? Title { get; set; } = string.Empty;
    public string? StudyCenter { get; set; } = string.Empty;
    public string? CountryOfStudy { get; set; } = string.Empty;
    public string? Source { get; set; } = string.Empty;
    public int IdTeacher { get; set; }
}