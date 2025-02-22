using Astragon.Configuration.Shared;
using Astragon.Model.Dtos.Research;
using Astragon.Model.Dtos.Teacher;

namespace Astragon.Modules.Research.Application.Port;

public interface IResearchOutPort : IBasePresenter<object>
{
    void GetAllResearchProject(IEnumerable<ResearchProjectDto> data);
    void GetAllScientificArticle(IEnumerable<ScientificArticleDto> data);
}