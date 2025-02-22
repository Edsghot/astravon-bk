using Astragon.Configuration.Shared;
using Astragon.Model.Dtos.User;

namespace Astragon.Modules.User.Application.Port;

public interface IUserOutPort : IBasePresenter<object>
{
    void GetAllUsersAsync(IEnumerable<UserDto> data);
}