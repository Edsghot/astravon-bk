using Astragon.Model.Dtos.Article;
using Astragon.Model.Dtos.project;
using Astragon.Model.Dtos.User;
using Astragon.Modules.User.Domain.Entity;

namespace Astragon.Modules.Research.Application.Port;

public interface IResearchInputPort
{
    Task GetAllResearchProject();
    Task GetAllScientificArticle();
    Task CreateResearchProjectAsync(CreateResearchProjectDto createDto);
    Task UpdateResearchProjectAsync(CreateResearchProjectDto updateDto);
    Task DeleteResearchProjectAsync(int id);
    Task GetResearchProjectByIdAsync(int id);

    ///article
    Task GetScientificArticleByIdAsync(int id);

    Task CreateScientificArticleAsync(CreateScientificArticleDto createDto);
    Task UpdateScientificArticleAsync(CreateScientificArticleDto updateDto);
    Task DeleteScientificArticleAsync(int id);
    Task GetResearchProjectsByTeacherIdAsync(int teacherId);
    Task GetScientificArticlesByTeacherIdAsync(int teacherId);
}