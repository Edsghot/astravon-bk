using Astragon.Configuration.Shared;
using Astragon.Model.Dtos.Research;
using Astragon.Model.Dtos.Teacher;
using Astragon.Modules.Research.Application.Port;
using Astragon.Modules.Teacher.Application.Port;

namespace Astragon.Modules.Research.Infraestructure.Presenter;

public class ResearchPresenter : BasePresenter<object>, IResearchOutPort
{
    public void GetAllResearchProject(IEnumerable<ResearchProjectDto> data)
    {
        Success(data, "Data encontrada");
    }

    public void GetAllScientificArticle(IEnumerable<ScientificArticleDto> data)
    {
        Success(data, "data encontrada");
    }
}