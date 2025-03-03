using Astravon.Configuration.Shared;
using Astravon.Model.Dtos.User;

namespace Astravon.Modules.User.Application.Port;

public interface IUserOutPort : IBasePresenter<object>
{
    void GetAllUsersAsync(IEnumerable<UserDto> data);
    void Login(UserDto data);
}