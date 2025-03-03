using Astravon.Model.Dtos.Teacher;

namespace Astravon.Modules.User.Application.Port;

public interface IUserInputPort
{
    Task GetAllUsersAsync();
    Task Login(LoginDto loginRequest);
    Task SendVerificationEmailAsync(string toEmail);
    Task ValidateCode(string email, string inputCode);
}