using Astragon.Model.Dtos.User;
using Astragon.Modules.User.Domain.Entity;

namespace Astragon.Modules.User.Application.Port;

public interface IUserInputPort
{
    Task GetAllUsersAsync();
}