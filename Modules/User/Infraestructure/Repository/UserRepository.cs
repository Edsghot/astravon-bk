using Astravon.Configuration.Context;
using Astravon.Configuration.Context.Repository;
using Astravon.Modules.User.Domain.Entity;
using Astravon.Modules.User.Domain.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Astravon.Modules.User.Infraestructure.Repository;

public class UserRepository : BaseRepository<MySqlContext>, IUserRepository
{
    public UserRepository(MySqlContext context) : base(context)
    {
    }

    public async Task<IEnumerable<UserEntity>> GetAllUserAsync()
    {
        return await _context.Users.ToListAsync();
    }
}