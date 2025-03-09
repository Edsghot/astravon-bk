namespace Astravon.Model.Dtos.User;

public record UserDto
{
    public int Id { get; set; }
    public string? FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; } = string.Empty;
    public string? Mail { get; set; } = string.Empty;
}