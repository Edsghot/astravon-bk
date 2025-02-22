using Astragon.Configuration.Context;
using Astragon.Configuration.Context.Repository;
using Astragon.Modules.Teacher.Domain.IRepository;

namespace Astragon.Modules.Teacher.Infraestructure.Repository;

public class TeacherRepository : BaseRepository<MySqlContext>, ITeacherRepository
{
    public TeacherRepository(MySqlContext context) : base(context)
    {
    }
}