using Astravon.Configuration.Shared;
using Astravon.Model.Dtos.User;
using Astravon.Modules.User.Application.Port;

namespace Astravon.Modules.User.Infraestructure.Presenter;

public class UserPresenter : BasePresenter<object>, IUserOutPort
{
    public void NotFound(string message = "Data not found")
    {
        base.NotFound(message);
    }
    
    public void GetAllUsersAsync(IEnumerable<UserDto> data)
    {
        Success(data, "Datos exitosos");
    }
    
    
    public void Login(UserDto data)
    {
        Success(data, "Datos exitosos");
    }
}