using Astragon.Configuration.Shared;
using Astragon.Model.Dtos.Teacher;
using Astragon.Modules.Teacher.Application.Port;

namespace Astragon.Modules.Teacher.Infraestructure.Presenter;

public class TeacherPresenter : BasePresenter<object>, ITeacherOutPort
{
    public void GetAllAsync(IEnumerable<TeacherDto> data)
    {
        Success(data, "Teacher successfully retrieved.");
    }

    public void GetById(TeacherDto data)
    {
        Success(data, "Teacher data");
    }

    public void Login(TeacherDto data)
    {
        Success(data, "teacher data");
    }
}