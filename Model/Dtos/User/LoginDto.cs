namespace Astravon.Model.Dtos.Teacher;

public record LoginDto
{
    public string Mail { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}