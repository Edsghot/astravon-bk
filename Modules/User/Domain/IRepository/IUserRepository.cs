using Astravon.Configuration.Context.Repository;
using Astravon.Modules.User.Domain.Entity;

namespace Astravon.Modules.User.Domain.IRepository;

public interface IUserRepository : IBaseRepository
{
    Task<IEnumerable<UserEntity>> GetAllUserAsync();
}