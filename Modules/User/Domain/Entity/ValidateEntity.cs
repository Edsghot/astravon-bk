namespace Astravon.Modules.User.Domain.Entity;

public class ValidateEntity
{
    public int IdValidate { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
}