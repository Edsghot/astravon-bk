namespace Astragon.Model.Dtos.Teacher;

public class UpdateTeacherDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; } = string.Empty;
    public string? Dni { get; set; } = string.Empty;
    public int? School { get; set; }
    public string? Mail { get; set; } = string.Empty;
    public string? Phone { get; set; } = string.Empty;
    public string? Password { get; set; } = string.Empty;
    public bool? Gender { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? RegistrationCode { get; set; } = string.Empty;
    public IFormFile? Image { get; set; }
    public string? Facebook { get; set; }
    public string? Description { get; set; } = string.Empty;
    public string? Instagram { get; set; }
    public string? LinkedIn { get; set; }
    public string? Orcid { get; set; }
    public string? Scopus { get; set; }
    public string? Concytec { get; set; }
    public string? Position { get; set; } = string.Empty;
}