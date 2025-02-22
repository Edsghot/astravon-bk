using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Astragon.Model.Dtos.Article;
using Astragon.Model.Dtos.project;
using Astragon.Model.Dtos.Research;
using Astragon.Model.Dtos.Teacher;
using Astragon.Modules.Research.Application.Port;
using Astragon.Modules.Research.Domain.Entity;
using Astragon.Modules.Research.Domain.IRepository;
using Astragon.Modules.Teacher.Application.Port;
using Astragon.Modules.Teacher.Domain.Entity;
using Astragon.Modules.Teacher.Domain.IRepository;
using ScientificArticleDto = Astragon.Model.Dtos.Research.ScientificArticleDto;

namespace Astragon.Modules.Research.Application.Adapter;

public class ResearchAdapter : IResearchInputPort
{
    private readonly IResearchOutPort _researchOutPort;
    private readonly IResearchRepository _researchRepository;

    private readonly DateTime _peruDateTime;
    private readonly Cloudinary _cloudinary;

    public ResearchAdapter(IResearchRepository repository, IResearchOutPort outPort)
    {
        _researchRepository = repository;
        _researchOutPort = outPort;
        var peruTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
        _peruDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, peruTimeZone);
        var account = new Account("dd0qlzyyk", "952839112726724", "7fxZGsz7Lz2vY5Ahp6spldgMTW4");
        _cloudinary = new Cloudinary(account);
    }

    public async Task CreateResearchProjectAsync(CreateResearchProjectDto createDto)
    {
        var project = new ResearchProjectEntity
        {
            Name = createDto.Title,
            Description = createDto.Description,
            IdTeacher = createDto.IdTeacher,
            Pdf = await UploadPdfAsync(createDto.File, "research"),
            Authors = createDto.Authors,
            Date = _peruDateTime,
            Summary = createDto.Summary,
            Year = createDto.Year
        };
        await _researchRepository.AddAsync(project);
        _researchOutPort.Ok("Proyecto de investigación creado exitosamente.");
    }

    public async Task UpdateResearchProjectAsync(CreateResearchProjectDto updateDto)
    {
        var researchProject = await _researchRepository.GetAsync<ResearchProjectEntity>(x => x.Id == updateDto.Id);
        if (researchProject == null)
        {
            _researchOutPort.NotFound("Proyecto de investigación no encontrado.");
            return;
        }

        researchProject.Name = updateDto.Title;
        researchProject.Description = updateDto.Description;
        researchProject.IdTeacher = updateDto.IdTeacher;
        researchProject.Authors = updateDto.Authors;
        researchProject.Date = _peruDateTime;
        researchProject.Summary = updateDto.Summary;
        researchProject.Year = updateDto.Year;

        if (updateDto.File != null) researchProject.Pdf = await UploadPdfAsync(updateDto.File, "research");

        await _researchRepository.UpdateAsync(researchProject);
        _researchOutPort.Ok("Proyecto de investigación actualizado exitosamente.");
    }

    public async Task DeleteResearchProjectAsync(int id)
    {
        var researchProject = await _researchRepository.GetAsync<ResearchProjectEntity>(x => x.Id == id);
        if (researchProject == null)
        {
            _researchOutPort.NotFound("Proyecto de investigación no encontrado.");
            return;
        }

        await _researchRepository.DeleteAsync(researchProject);
        _researchOutPort.Ok("Proyecto de investigación eliminado exitosamente.");
    }

    public async Task GetAllResearchProject()
    {
        var research = await _researchRepository.GetAllAsync<ResearchProjectEntity>();

        var researchEntities = research.ToList();
        if (!researchEntities.Any())
        {
            _researchOutPort.NotFound("No se encontraron proyectos de investigacion");
            return;
        }

        var researchDtos = researchEntities.Adapt<List<ResearchProjectDto>>();

        _researchOutPort.GetAllResearchProject(researchDtos);
    }

    public async Task GetResearchProjectByIdAsync(int id)
    {
        var researchProject = await _researchRepository.GetAsync<ResearchProjectEntity>(x => x.Id == id);
        if (researchProject == null)
        {
            _researchOutPort.NotFound("Proyecto de investigación no encontrado.");
            return;
        }

        var researchDto = researchProject.Adapt<ResearchProjectDto>();
        _researchOutPort.Success(researchDto, "data encontrada");
    }


    //++++++++++++++++++++++++++++++++++++++++++ARTICLE

    public async Task GetAllScientificArticle()
    {
        var research = await _researchRepository.GetAllAsync<ScientificArticleEntity>();

        var researchEntities = research.ToList();
        if (!researchEntities.Any())
        {
            _researchOutPort.NotFound("No se encontro articulos");
            return;
        }

        var researchDtos = researchEntities.Adapt<List<ScientificArticleDto>>();

        _researchOutPort.GetAllScientificArticle(researchDtos);
    }

    public async Task GetScientificArticleByIdAsync(int id)
    {
        var article = await _researchRepository.GetAsync<ScientificArticleEntity>(x => x.Id == id);
        if (article == null)
        {
            _researchOutPort.NotFound("Artículo científico no encontrado.");
            return;
        }

        var articleDto = article.Adapt<ScientificArticleDto>();
        _researchOutPort.Success(articleDto, "Artículo encontrado.");
    }

    public async Task CreateScientificArticleAsync(CreateScientificArticleDto createDto)
    {
        var article = createDto.Adapt<ScientificArticleEntity>();
        await _researchRepository.AddAsync(article);
        _researchOutPort.Ok("Artículo científico creado exitosamente.");
    }

    public async Task UpdateScientificArticleAsync(CreateScientificArticleDto updateDto)
    {
        var article = await _researchRepository.GetAsync<ScientificArticleEntity>(x => x.Id == updateDto.Id);

        var teacher = article.IdTeacher;
        if (article == null)
        {
            _researchOutPort.NotFound("Artículo científico no encontrado.");
            return;
        }

        article = updateDto.Adapt(article);
        article.IdTeacher = teacher;
        await _researchRepository.UpdateAsync(article);
        _researchOutPort.Ok("Artículo científico actualizado exitosamente.");
    }

    public async Task DeleteScientificArticleAsync(int id)
    {
        var article = await _researchRepository.GetAsync<ScientificArticleEntity>(x => x.Id == id);
        if (article == null)
        {
            _researchOutPort.NotFound("Artículo científico no encontrado.");
            return;
        }

        await _researchRepository.DeleteAsync(article);
        _researchOutPort.Ok("Artículo científico eliminado exitosamente.");
    }

    public async Task GetResearchProjectsByTeacherIdAsync(int teacherId)
    {
        var researchProjects =
            await _researchRepository.GetAllAsync<ResearchProjectEntity>(x => x.Where(x => x.IdTeacher == teacherId));
        var researchProjectEntities = researchProjects.ToList();
        if (!researchProjectEntities.Any())
        {
            _researchOutPort.NotFound("No se encontraron proyectos de investigación para el docente.");
            return;
        }

        var researchProjectDtos = researchProjectEntities.Adapt<List<ResearchProjectDto>>();
        _researchOutPort.Success(researchProjectDtos, "data");
    }


    public async Task GetScientificArticlesByTeacherIdAsync(int teacherId)
    {
        var articles =
            await _researchRepository.GetAllAsync<ScientificArticleEntity>(x => x.Where(x => x.IdTeacher == teacherId));
        var articleEntities = articles.ToList();
        if (!articleEntities.Any())
        {
            _researchOutPort.NotFound("No se encontraron artículos científicos para el docente.");
            return;
        }

        var articleDtos = articleEntities.Adapt<List<ScientificArticleDto>>();
        _researchOutPort.Success(articleDtos, "data");
    }

    private async Task<string> UploadPdfAsync(IFormFile file, string folder)
    {
        try
        {
            var account = new Account("dd0qlzyyk", "952839112726724", "7fxZGsz7Lz2vY5Ahp6spldgMTW4");
            var cloudinary = new Cloudinary(account);
            cloudinary.Api.Secure = true; // Asegurar HTTPS

            await using var stream = file.OpenReadStream();

            var uploadParams = new RawUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = folder,
                Type = "upload", // Asegura que sea accesible públicamente
                Overwrite = true, // Reemplazar archivo si ya existe
                UseFilename = true, // Mantener nombre original
                UniqueFilename = false // Evita sufijos aleatorios en el nombre
            };

            var uploadResult = await cloudinary.UploadAsync(uploadParams);

            if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                return uploadResult.SecureUrl.AbsoluteUri; // Devuelve URL accesible públicamente

            return "";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al subir el archivo PDF: {ex.Message}");
            return "";
        }
    }
}