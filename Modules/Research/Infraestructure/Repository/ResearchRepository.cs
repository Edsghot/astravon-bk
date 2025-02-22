using Astragon.Configuration.Context;
using Astragon.Configuration.Context.Repository;
using Astragon.Modules.Research.Domain.IRepository;
using Astragon.Modules.Teacher.Domain.IRepository;

namespace Astragon.Modules.Research.Infraestructure.Repository;

public class ResearchRepository : BaseRepository<MySqlContext>, IResearchRepository
{
    public ResearchRepository(MySqlContext context) : base(context)
    {
    }
}