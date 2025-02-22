using Astragon.Model.Dtos.AcademicFormation;
using Astragon.Model.Dtos.Teacher;
using Astragon.Model.Dtos.TeachingExperienceDto;
using Astragon.Model.Dtos.ThesisAdvisingExperience;
using Astragon.Model.Dtos.User;
using Astragon.Model.Dtos.WorkExperience;
using Astragon.Modules.User.Domain.Entity;
using TeachingExperienceDto = Astragon.Model.Dtos.Teacher.TeachingExperienceDto;
using ThesisAdvisingExperienceDto = Astragon.Model.Dtos.Teacher.ThesisAdvisingExperienceDto;
using WorkExperienceDto = Astragon.Model.Dtos.Teacher.WorkExperienceDto;

namespace Astragon.Modules.Teacher.Application.Port;

public interface ITeacherInputPort
{
    Task GetAllAsync();
    Task GetById(int id);
    Task Login(LoginDto loginRequest);
    Task SendVerificationEmailAsync(string toEmail);
    Task ValidateCode(string email, string inputCode);

    Task CreateTeacherAsync(CreateTeacherDto teacherDto);

    //teaching experiencia
    Task GetAllTeachingExperiencesByTeacherIdAsync(int teacherId);
    Task GetTeachingExperienceByIdAsync(int id);
    Task CreateTeachingExperienceAsync(CreateTeachingExperienceDto createDto);
    Task UpdateTeachingExperienceAsync(TeachingExperienceDto updateDto);
    Task DeleteTeachingExperienceAsync(int id);

    //teaching advising thesis experiencia
    Task GetAllThesisAdvisingExperiencesByTeacherIdAsync(int teacherId);
    Task GetThesisAdvisingExperienceByIdAsync(int id);
    Task CreateThesisAdvisingExperienceAsync(CreateThesisAdvisingExperienceDto createDto);
    Task UpdateThesisAdvisingExperienceAsync(ThesisAdvisingExperienceDto updateDto);
    Task DeleteThesisAdvisingExperienceAsync(int id);

    //Teaching advising ...........

    Task GetAllWorkExperiencesByTeacherIdAsync(int teacherId);
    Task GetWorkExperienceByIdAsync(int id);
    Task CreateWorkExperienceAsync(CreateWorkExperienceDto createDto);
    Task UpdateWorkExperienceAsync(WorkExperienceDto updateDto);
    Task DeleteWorkExperienceAsync(int id);
    Task GetTeacherStatsAsync(int teacherId);
    Task UpdateTeacherAsync(UpdateTeacherDto updateDto);
    Task GetAllEducationFormationsByTeacherIdAsync(int teacherId);
    Task GetEducationFormationByIdAsync(int id);
    Task CreateEducationFormationAsync(CreateAcademicFormationDto createDto);
    Task UpdateEducationFormationAsync(UpdateAcademicFormationDto updateDto);
    Task DeleteEducationFormationAsync(int id);
}