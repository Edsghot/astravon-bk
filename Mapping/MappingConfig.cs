using Mapster;
using Astragon.Model.Dtos.Teacher;
using Astragon.Modules.Teacher.Domain.Entity;

namespace Astragon.Mapping;

public class MappingConfig
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<TeacherEntity, TeacherDto>.NewConfig();
        TypeAdapterConfig<WorkExperienceEntity, WorkExperienceDto>.NewConfig();
        TypeAdapterConfig<TeachingExperienceEntity, TeachingExperienceDto>.NewConfig();
        TypeAdapterConfig<ThesisAdvisingExperienceEntity, ThesisAdvisingExperienceDto>.NewConfig();
    }
}