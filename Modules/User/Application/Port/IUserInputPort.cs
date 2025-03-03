using Astravon.Model.Dtos.Teacher;
using Astravon.Model.Dtos.User;

namespace Astravon.Modules.User.Application.Port;

public interface IUserInputPort
{
    Task GetAllUsersAsync();
    Task Login(LoginDto loginRequest);
    Task CreateUser(CreateUserDto createUserRequest);
    Task SendVerificationEmailAsync(string toEmail);
    Task ValidateCode(string email, string inputCode);
}