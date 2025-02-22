using Microsoft.AspNetCore.Mvc;
using Astragon.Model.Dtos.Article;
using Astragon.Model.Dtos.project;
using Astragon.Modules.Research.Application.Port;
using Astragon.Modules.Teacher.Application.Port;

namespace Astragon.Modules.Research.Infraestructure.Controller;

[Route("api/[controller]")]
[ApiController]
public class ResearchController : ControllerBase
{
    private readonly IResearchInputPort _researchInputPort;
    private readonly IResearchOutPort _researchOutPort;

    public ResearchController(IResearchInputPort inputPort, IResearchOutPort outPort)
    {
        _researchInputPort = inputPort;
        _researchOutPort = outPort;
    }

    [HttpGet("GetAllResearchProject")]
    public async Task<IActionResult> GetAllResearchProject()
    {
        await _researchInputPort.GetAllResearchProject();
        var response = _researchOutPort.GetResponse;

        return Ok(response);
    }

    [HttpGet("GetResearchProjectById/{id}")]
    public async Task<IActionResult> GetResearchProjectById(int id)
    {
        await _researchInputPort.GetResearchProjectByIdAsync(id);
        var response = _researchOutPort.GetResponse;

        return Ok(response);
    }

    [HttpPost("CreateResearchProject")]
    public async Task<IActionResult> CreateResearchProject([FromForm] CreateResearchProjectDto createDto)
    {
        await _researchInputPort.CreateResearchProjectAsync(createDto);
        var response = _researchOutPort.GetResponse;

        return Ok(response);
    }

    [HttpPut("UpdateResearchProject")]
    public async Task<IActionResult> UpdateResearchProject([FromForm] CreateResearchProjectDto updateDto)
    {
        await _researchInputPort.UpdateResearchProjectAsync(updateDto);
        var response = _researchOutPort.GetResponse;

        return Ok(response);
    }

    [HttpDelete("DeleteResearchProject/{id}")]
    public async Task<IActionResult> DeleteResearchProject(int id)
    {
        await _researchInputPort.DeleteResearchProjectAsync(id);
        var response = _researchOutPort.GetResponse;

        return Ok(response);
    }
    //+++++++++++++++++++++++++++++++++++++++++ ARTICLE

    [HttpGet("GetAllScientificArticle")]
    public async Task<IActionResult> GetAllScientificArticle()
    {
        await _researchInputPort.GetAllScientificArticle();
        var response = _researchOutPort.GetResponse;

        return Ok(response);
    }


    [HttpGet("GetScientificArticleById/{id}")]
    public async Task<IActionResult> GetScientificArticleById(int id)
    {
        await _researchInputPort.GetScientificArticleByIdAsync(id);
        var response = _researchOutPort.GetResponse;

        return Ok(response);
    }

    [HttpPost("CreateScientificArticle")]
    public async Task<IActionResult> CreateScientificArticle([FromBody] CreateScientificArticleDto createDto)
    {
        await _researchInputPort.CreateScientificArticleAsync(createDto);
        var response = _researchOutPort.GetResponse;

        return Ok(response);
    }

    [HttpPut("UpdateScientificArticle")]
    public async Task<IActionResult> UpdateScientificArticle([FromBody] CreateScientificArticleDto updateDto)
    {
        await _researchInputPort.UpdateScientificArticleAsync(updateDto);
        var response = _researchOutPort.GetResponse;

        return Ok(response);
    }

    [HttpDelete("DeleteScientificArticle/{id}")]
    public async Task<IActionResult> DeleteScientificArticle(int id)
    {
        await _researchInputPort.DeleteScientificArticleAsync(id);
        var response = _researchOutPort.GetResponse;

        return Ok(response);
    }

    [HttpGet("GetResearchProjectsByTeacherId/{teacherId}")]
    public async Task<IActionResult> GetResearchProjectsByTeacherId(int teacherId)
    {
        await _researchInputPort.GetResearchProjectsByTeacherIdAsync(teacherId);
        var response = _researchOutPort.GetResponse;

        return Ok(response);
    }

    [HttpGet("GetScientificArticlesByTeacherId/{teacherId}")]
    public async Task<IActionResult> GetScientificArticlesByTeacherId(int teacherId)
    {
        await _researchInputPort.GetScientificArticlesByTeacherIdAsync(teacherId);
        var response = _researchOutPort.GetResponse;

        return Ok(response);
    }
}