using Microsoft.EntityFrameworkCore;
using Astragon.Configuration.Context;
using Astragon.Configuration.Context.Repository;
using Astragon.Modules.User.Domain.Entity;
using Astragon.Modules.User.Domain.IRepository;

namespace Astragon.Modules.User.Infraestructure.Repository;

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