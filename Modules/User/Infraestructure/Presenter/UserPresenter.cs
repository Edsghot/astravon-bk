using Astragon.Model.Dtos.Response;
using Astragon.Configuration.Shared;
using Astragon.Model.Dtos.User;
using Astragon.Modules.User.Application.Port;

namespace Astragon.Modules.User.Infraestructure.Presenter;

public class UserPresenter : BasePresenter<object>, IUserOutPort
{
    public void GetAllUsersAsync(IEnumerable<UserDto> data)
    {
        Success(data, "Users successfully retrieved.");
    }

    public void NotFound(string message = "Data not found")
    {
        base.NotFound(message);
    }
}