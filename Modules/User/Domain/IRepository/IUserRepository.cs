using Astragon.Configuration.Context.Repository;
using Astragon.Modules.User.Domain.Entity;

namespace Astragon.Modules.User.Domain.IRepository;

public interface IUserRepository : IBaseRepository
{
    Task<IEnumerable<UserEntity>> GetAllUserAsync();
}