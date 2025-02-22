using System.Net;
using System.Net.Mail;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Astragon.Model.Dtos.Teacher;
using Astragon.Modules.Teacher.Application.Port;
using Astragon.Modules.Teacher.Domain.Entity;
using Astragon.Modules.Teacher.Domain.IRepository;
using MailKit.Net.Smtp;
using MimeKit;
using Astragon.Model.Dtos.AcademicFormation;
using Astragon.Model.Dtos.TeachingExperienceDto;
using Astragon.Model.Dtos.ThesisAdvisingExperience;
using Astragon.Model.Dtos.WorkExperience;
using Astragon.Modules.Research.Domain.Entity;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using TeachingExperienceDto = Astragon.Model.Dtos.Teacher.TeachingExperienceDto;
using ThesisAdvisingExperienceDto = Astragon.Model.Dtos.Teacher.ThesisAdvisingExperienceDto;
using WorkExperienceDto = Astragon.Model.Dtos.Teacher.WorkExperienceDto;

namespace Astragon.Modules.Teacher.Application.Adapter;

public class TeacherAdapter : ITeacherInputPort
{
    private readonly ITeacherOutPort _teacherOutPort;
    private readonly ITeacherRepository _teacherRepository;
    private readonly string _smtpServer = "smtp.gmail.com";
    private readonly int _smtpPort = 587;
    private readonly string _smtpUser = "edsghotSolutions@gmail.com";
    private readonly string _smtpPass = "lfqpacmpmnvuwhvb";

    private readonly Cloudinary _cloudinary;


    public TeacherAdapter(ITeacherRepository repository, ITeacherOutPort teacherOutPort)
    {
        _teacherRepository = repository;
        _teacherOutPort = teacherOutPort;
        var account = new Account("dd0qlzyyk", "952839112726724", "7fxZGsz7Lz2vY5Ahp6spldgMTW4");
        _cloudinary = new Cloudinary(account);
    }

    public async Task GetById(int id)
    {
        var teachers = await _teacherRepository.GetAsync<TeacherEntity>(
            x => x.Id == id,
            query => query
                .Include(t => t.TeachingExperiences)
                .Include(t => t.WorkExperiences)
                .Include(t => t.ThesisAdvisingExperiences).AsNoTracking()
        );
        if (teachers == null)
        {
            _teacherOutPort.NotFound("No teacher found.");
            return;
        }

        var teacherDtos = teachers.Adapt<TeacherDto>();
        _teacherOutPort.GetById(teacherDtos);
    }

    public async Task GetAllAsync()
    {
        var teachers = await _teacherRepository.GetAllAsync<TeacherEntity>(query => query
            .Include(t => t.TeachingExperiences)
            .Include(t => t.WorkExperiences)
            .Include(t => t.ThesisAdvisingExperiences).AsNoTracking()
        );

        var teacherEntities = teachers.ToList();
        if (!teacherEntities.Any())
        {
            _teacherOutPort.NotFound("No teacher found.");
            return;
        }

        var teacherDtos = teachers.Adapt<List<TeacherDto>>();

        _teacherOutPort.GetAllAsync(teacherDtos);
    }

    public async Task Login(LoginDto loginRequest)
    {
        var user = await _teacherRepository.GetAsync<TeacherEntity>(x =>
            x.Mail == loginRequest.Email && x.Password == loginRequest.Password);
        if (user == null)
        {
            _teacherOutPort.Error("Esta mal tus credenciales");
            return;
        }

        var teacher = user.Adapt<TeacherDto>();

        _teacherOutPort.Login(teacher);
    }


    private static string GenerateCode()
    {
        var random = new Random();
        return random.Next(1000, 9999).ToString();
    }

    public async Task SendVerificationEmailAsync(string toEmail)
    {
        var code = GenerateCode();

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("UnambaVirtual", _smtpUser));
        message.To.Add(new MailboxAddress("", toEmail));
        message.Subject = "Código de verificación";

        message.Body = new TextPart("html")
        {
            Text = $"<h2>Tu código de verificación es: <b>{code}</b></h2><p>Este código expira en unos minutos.</p>"
        };

        using var client = new SmtpClient();
        try
        {
            await client.ConnectAsync(_smtpServer, _smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_smtpUser, _smtpPass);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);

            var validatioon = await _teacherRepository.GetAsync<ValidateEntity>(x => x.Email == toEmail);
            if (validatioon != null)
            {
                validatioon.Code = code;
                await _teacherRepository.UpdateAsync(validatioon);
            }
            else
            {
                var verification = new ValidateEntity
                {
                    Email = toEmail,
                    Code = code
                };
                await _teacherRepository.AddAsync(verification);
            }

            _teacherOutPort.Ok("Código de verificación enviado");
        }
        catch (Exception ex)
        {
            _teacherOutPort.Error("Error: " + ex.Message);
        }
    }

    public async Task ValidateCode(string email, string inputCode)
    {
        var data = await _teacherRepository.GetAsync<ValidateEntity>(x => x.Email == email && x.Code == inputCode);

        if (data == null)
        {
            _teacherOutPort.Error("El codigo es incorrecto intente de nuevo!");
            return;
        }

        _teacherOutPort.Ok("El codigo es correcto");
    }

    public async Task CreateTeacherAsync(CreateTeacherDto teacherDto)
    {
        var teacher = new TeacherEntity
        {
            FirstName = teacherDto.FirstName,
            LastName = teacherDto.LastName,
            Dni = teacherDto.Dni, // ✅ Agregado
            School = teacherDto.School ?? 0, // ✅ Agregado
            Mail = teacherDto.Mail,
            Password = teacherDto.Password,
            RegistrationCode = teacherDto.RegistrationCode ?? string.Empty,
            Image = teacherDto.Image,
            Facebook = teacherDto.Facebook,
            Description = teacherDto.Description ?? string.Empty,
            LinkedIn = teacherDto.LinkedIn,
            Orcid = teacherDto.Orcid, // ✅ Agregado
            Scopus = teacherDto.Scopus, // ✅ Agregado
            Concytec = teacherDto.Concytec, // ✅ Agregado

            // Campos adicionales en el Entity que no vienen del DTO
            Phone = string.Empty, // No viene del DTO, se deja vacío
            Gender = false, // No viene del DTO, valor por defecto
            BirthDate = DateTime.MinValue, // No viene del DTO, valor por defecto
            Instagram = null, // No viene del DTO, se deja como null
            Position = string.Empty, // No viene del DTO, se deja vacío
            WorkExperiences = new List<WorkExperienceEntity>(), // Se inicializa vacío
            TeachingExperiences = new List<TeachingExperienceEntity>(), // Se inicializa vacío
            ThesisAdvisingExperiences = new List<ThesisAdvisingExperienceEntity>() // Se inicializa vacío
        };

        await _teacherRepository.AddAsync(teacher);

        _teacherOutPort.Ok("Se creó correctamente el docente.");
    }

    /// <summary>
    /// //////////////////
    /// </summary>
    /// <param name="teacherId"></param>

    #region Teaching Data

    public async Task GetAllTeachingExperiencesByTeacherIdAsync(int teacherId)
    {
        var experiences =
            await _teacherRepository.GetAllAsync<TeachingExperienceEntity>(x => x.Where(x => x.TeacherId == teacherId));
        var experienceEntities = experiences.ToList();
        if (!experienceEntities.Any())
        {
            _teacherOutPort.NotFound("No se encontraron experiencias de enseñanza para el docente.");
            return;
        }

        var experienceDtos = experienceEntities.Adapt<List<TeachingExperienceDto>>();
        _teacherOutPort.Success(experienceDtos, "data");
    }

    public async Task GetTeachingExperienceByIdAsync(int id)
    {
        var experience = await _teacherRepository.GetAsync<TeachingExperienceEntity>(x => x.Id == id);
        if (experience == null)
        {
            _teacherOutPort.NotFound("Experiencia de enseñanza no encontrada.");
            return;
        }

        var experienceDto = experience.Adapt<TeachingExperienceDto>();
        _teacherOutPort.Success(experienceDto, "data");
    }

    public async Task CreateTeachingExperienceAsync(CreateTeachingExperienceDto createDto)
    {
        var experience = createDto.Adapt<TeachingExperienceEntity>();
        await _teacherRepository.AddAsync(experience);
        _teacherOutPort.Ok("Experiencia de enseñanza creada exitosamente.");
    }

    public async Task UpdateTeachingExperienceAsync(TeachingExperienceDto updateDto)
    {
        var experience = await _teacherRepository.GetAsync<TeachingExperienceEntity>(x => x.Id == updateDto.Id);
        if (experience == null)
        {
            _teacherOutPort.NotFound("Experiencia de enseñanza no encontrada.");
            return;
        }

        experience = updateDto.Adapt(experience);
        await _teacherRepository.UpdateAsync(experience);
        _teacherOutPort.Ok("Experiencia de enseñanza actualizada exitosamente.");
    }

    public async Task DeleteTeachingExperienceAsync(int id)
    {
        var experience = await _teacherRepository.GetAsync<TeachingExperienceEntity>(x => x.Id == id);
        if (experience == null)
        {
            _teacherOutPort.NotFound("Experiencia de enseñanza no encontrada.");
            return;
        }

        await _teacherRepository.DeleteAsync(experience);
        _teacherOutPort.Ok("Experiencia de enseñanza eliminada exitosamente.");
    }

    #endregion

    #region Teaching Advising data

    public async Task GetAllThesisAdvisingExperiencesByTeacherIdAsync(int teacherId)
    {
        var experiences =
            await _teacherRepository.GetAllAsync<ThesisAdvisingExperienceEntity>(x =>
                x.Where(x => x.TeacherId == teacherId));
        var experienceEntities = experiences.ToList();
        if (!experienceEntities.Any())
        {
            _teacherOutPort.NotFound("No se encontraron experiencias de asesoría de tesis para el docente.");
            return;
        }

        var experienceDtos = experienceEntities.Adapt<List<ThesisAdvisingExperienceDto>>();
        _teacherOutPort.Success(experienceDtos, "data");
    }

    public async Task GetThesisAdvisingExperienceByIdAsync(int id)
    {
        var experience = await _teacherRepository.GetAsync<ThesisAdvisingExperienceEntity>(x => x.Id == id);
        if (experience == null)
        {
            _teacherOutPort.NotFound("Experiencia de asesoría de tesis no encontrada.");
            return;
        }

        var experienceDto = experience.Adapt<ThesisAdvisingExperienceDto>();
        _teacherOutPort.Success(experienceDto, "data");
    }

    public async Task CreateThesisAdvisingExperienceAsync(CreateThesisAdvisingExperienceDto createDto)
    {
        var experience = createDto.Adapt<ThesisAdvisingExperienceEntity>();
        await _teacherRepository.AddAsync(experience);
        _teacherOutPort.Ok("Experiencia de asesoría de tesis creada exitosamente.");
    }

    public async Task UpdateThesisAdvisingExperienceAsync(ThesisAdvisingExperienceDto updateDto)
    {
        var experience = await _teacherRepository.GetAsync<ThesisAdvisingExperienceEntity>(x => x.Id == updateDto.Id);
        if (experience == null)
        {
            _teacherOutPort.NotFound("Experiencia de asesoría de tesis no encontrada.");
            return;
        }

        experience = updateDto.Adapt(experience);
        await _teacherRepository.UpdateAsync(experience);
        _teacherOutPort.Ok("Experiencia de asesoría de tesis actualizada exitosamente.");
    }

    public async Task DeleteThesisAdvisingExperienceAsync(int id)
    {
        var experience = await _teacherRepository.GetAsync<ThesisAdvisingExperienceEntity>(x => x.Id == id);
        if (experience == null)
        {
            _teacherOutPort.NotFound("Experiencia de asesoría de tesis no encontrada.");
            return;
        }

        await _teacherRepository.DeleteAsync(experience);
        _teacherOutPort.Ok("Experiencia de asesoría de tesis eliminada exitosamente.");
    }

    #endregion

    #region Work expericie

    public async Task GetAllWorkExperiencesByTeacherIdAsync(int teacherId)
    {
        var experiences =
            await _teacherRepository.GetAllAsync<WorkExperienceEntity>(x => x.Where(x => x.TeacherId == teacherId));
        var experienceEntities = experiences.ToList();
        if (!experienceEntities.Any())
        {
            _teacherOutPort.NotFound("No se encontraron experiencias laborales para el docente.");
            return;
        }

        var experienceDtos = experienceEntities.Adapt<List<WorkExperienceDto>>();
        _teacherOutPort.Success(experienceDtos, "data");
    }

    public async Task GetWorkExperienceByIdAsync(int id)
    {
        var experience = await _teacherRepository.GetAsync<WorkExperienceEntity>(x => x.Id == id);
        if (experience == null)
        {
            _teacherOutPort.NotFound("Experiencia laboral no encontrada.");
            return;
        }

        var experienceDto = experience.Adapt<WorkExperienceDto>();
        _teacherOutPort.Success(experienceDto, "data");
    }

    public async Task CreateWorkExperienceAsync(CreateWorkExperienceDto createDto)
    {
        var experience = createDto.Adapt<WorkExperienceEntity>();
        await _teacherRepository.AddAsync(experience);
        _teacherOutPort.Ok("Experiencia laboral creada exitosamente.");
    }

    public async Task UpdateWorkExperienceAsync(WorkExperienceDto updateDto)
    {
        var experience = await _teacherRepository.GetAsync<WorkExperienceEntity>(x => x.Id == updateDto.Id);
        if (experience == null)
        {
            _teacherOutPort.NotFound("Experiencia laboral no encontrada.");
            return;
        }

        experience = updateDto.Adapt(experience);
        await _teacherRepository.UpdateAsync(experience);
        _teacherOutPort.Ok("Experiencia laboral actualizada exitosamente.");
    }

    public async Task DeleteWorkExperienceAsync(int id)
    {
        var experience = await _teacherRepository.GetAsync<WorkExperienceEntity>(x => x.Id == id);
        if (experience == null)
        {
            _teacherOutPort.NotFound("Experiencia laboral no encontrada.");
            return;
        }

        await _teacherRepository.DeleteAsync(experience);
        _teacherOutPort.Ok("Experiencia laboral eliminada exitosamente.");
    }

    public async Task UpdateTeacherAsync(UpdateTeacherDto updateDto)
    {
        var teacher = await _teacherRepository.GetAsync<TeacherEntity>(x => x.Id == updateDto.Id);
        if (teacher == null)
        {
            _teacherOutPort.NotFound("Docente no encontrado.");
            return;
        }

        teacher.FirstName = updateDto.FirstName ?? teacher.FirstName;
        teacher.LastName = updateDto.LastName ?? teacher.LastName;
        teacher.Dni = updateDto.Dni ?? teacher.Dni;
        teacher.School = updateDto.School ?? teacher.School;
        teacher.Mail = updateDto.Mail ?? teacher.Mail;
        teacher.Phone = updateDto.Phone ?? teacher.Phone;
        teacher.Password = updateDto.Password ?? teacher.Password;
        teacher.Gender = updateDto.Gender ?? teacher.Gender;
        teacher.BirthDate = updateDto.BirthDate ?? teacher.BirthDate;
        teacher.RegistrationCode = updateDto.RegistrationCode ?? teacher.RegistrationCode;
        teacher.Facebook = updateDto.Facebook ?? teacher.Facebook;
        teacher.Description = updateDto.Description ?? teacher.Description;
        teacher.Instagram = updateDto.Instagram ?? teacher.Instagram;
        teacher.LinkedIn = updateDto.LinkedIn ?? teacher.LinkedIn;
        teacher.Orcid = updateDto.Orcid ?? teacher.Orcid;
        teacher.Scopus = updateDto.Scopus ?? teacher.Scopus;
        teacher.Concytec = updateDto.Concytec ?? teacher.Concytec;
        teacher.Position = updateDto.Position ?? teacher.Position;

        if (updateDto.Image != null) teacher.Image = await UploadImage(updateDto.Image, "teacher");

        await _teacherRepository.UpdateAsync(teacher);
        _teacherOutPort.Ok("Docente actualizado exitosamente.");
    }

    private async Task<string> UploadImage(IFormFile file, string folder)
    {
        await using var streamCover = file.OpenReadStream();
        var uploadParamsCover = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, streamCover),
            Transformation = new Transformation().Width(500).Height(500).Crop("fill"),
            Folder = folder
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParamsCover);

        if (uploadResult.StatusCode == HttpStatusCode.OK) return uploadResult.Url.AbsoluteUri;
        return "";
    }

    public async Task GetTeacherStatsAsync(int teacherId)
    {
        var teacher = await _teacherRepository.GetAsync<TeacherEntity>(x => x.Id == teacherId);
        if (teacher == null)
        {
            _teacherOutPort.NotFound("Docente no encontrado.");
            return;
        }

        var user = new UserDto
        {
            Name = teacher.FirstName + " " + teacher.LastName,
            ProfileImage = teacher.Image
        };

        var projects =
            await _teacherRepository.GetAllAsync<ResearchProjectEntity>(x => x.Where(x => x.IdTeacher == teacherId));
        var articles =
            await _teacherRepository.GetAllAsync<ScientificArticleEntity>(x => x.Where(x => x.IdTeacher == teacherId));

        var stats = new List<StatDto>
        {
            new()
            {
                Title = "Total de proyectos", Total = projects.Count().ToString(), Rate = "0.43%", LevelUp = true,
                Icon = "BookCopy"
            },
            new()
            {
                Title = "Total de artículos", Total = articles.Count().ToString(), Rate = "4.35%", LevelUp = true,
                Icon = "Newspaper"
            },

            new()
            {
                Title = "Ultimos proyectos", Total = "2", Rate = "2.35%", LevelUp = true,
                Icon = "CalendarArrowUp"
            },

        };

        var projectData = new int[12];
        var articleData = new int[12];

        foreach (var project in projects) projectData[project.Date.Month - 1]++;

        foreach (var article in articles) articleData[article.Date.Month - 1]++;

        var grafico = new GraficoDto
        {
            Series = new List<SeriesDto>
            {
                new()
                {
                    Name = "Proyectos",
                    Data = projectData.ToList()
                },
                new()
                {
                    Name = "Artículos",
                    Data = articleData.ToList()
                }
            }
        };

        var tabla = new List<TablaDto>();

        var latestProjects = projects.OrderByDescending(p => p.Date).Take(2);
        var latestArticles = articles.OrderByDescending(a => a.Date).Take(2);

        tabla.AddRange(latestProjects.Select(p => new TablaDto
            { Name = p.Name, Date = p.Date.ToString("dd/MM/yyyy") }));
        tabla.AddRange(latestArticles.Select(a => new TablaDto
            { Name = a.Name, Date = a.Date.ToString("dd/MM/yyyy") }));

        var dataRes = new ReportDataDto
        {
            User = user,
            Stats = stats,
            Grafico = grafico,
            Tabla = tabla
        };

        _teacherOutPort.Success(dataRes, "data");
    }

    #endregion

    //===========================================Formacion academica
    public async Task GetAllEducationFormationsByTeacherIdAsync(int teacherId)
    {
        var formations =
            await _teacherRepository.GetAllAsync<AcademicFormationEntity>(x => x.Where(x => x.IdTeacher == teacherId));
        var formationDtos = formations.Adapt<List<UpdateAcademicFormationDto>>();
        _teacherOutPort.Success(formationDtos, "DATA");
    }

    public async Task GetEducationFormationByIdAsync(int id)
    {
        var formation = await _teacherRepository.GetAsync<AcademicFormationEntity>(x => x.Id == id);
        if (formation == null)
        {
            _teacherOutPort.NotFound("Formación académica no encontrada.");
            return;
        }

        var formationDto = formation.Adapt<UpdateAcademicFormationDto>();
        _teacherOutPort.Success(formationDto, "DATA");
    }

    public async Task CreateEducationFormationAsync(CreateAcademicFormationDto createDto)
    {
        var formation = createDto.Adapt<AcademicFormationEntity>();
        await _teacherRepository.AddAsync(formation);
        _teacherOutPort.Ok("Formación académica creada exitosamente.");
    }

    public async Task UpdateEducationFormationAsync(UpdateAcademicFormationDto updateDto)
    {
        var formation = await _teacherRepository.GetAsync<AcademicFormationEntity>(x => x.Id == updateDto.Id);
        if (formation == null)
        {
            _teacherOutPort.NotFound("Formación académica no encontrada.");
            return;
        }

        formation = updateDto.Adapt(formation);
        await _teacherRepository.UpdateAsync(formation);
        _teacherOutPort.Ok("Formación académica actualizada exitosamente.");
    }

    public async Task DeleteEducationFormationAsync(int id)
    {
        var formation = await _teacherRepository.GetAsync<AcademicFormationEntity>(x => x.Id == id);
        if (formation == null)
        {
            _teacherOutPort.NotFound("Formación académica no encontrada.");
            return;
        }

        await _teacherRepository.DeleteAsync(formation);
        _teacherOutPort.Ok("Formación académica eliminada exitosamente.");
    }
}