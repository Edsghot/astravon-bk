namespace Astravon.Model.Dtos.User;

public class CreateUserDto
{
    public string? FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; } = string.Empty;
    public string? Mail { get; set; } = string.Empty;
    public string? Password { get; set; } = string.Empty;
}