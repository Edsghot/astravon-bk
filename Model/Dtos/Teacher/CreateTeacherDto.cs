namespace Astragon.Model.Dtos.Teacher;

public record CreateTeacherDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Dni { get; set; }
    public int? School { get; set; }
    public string? Mail { get; set; }
    public string? Orcid { get; set; }
    public string? Scopus { get; set; }
    public string? Concytec { get; set; }
    public string? RegistrationCode { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }
    public string? Password { get; set; }
    public string? LinkedIn { get; set; }
    public string? Facebook { get; set; }
}