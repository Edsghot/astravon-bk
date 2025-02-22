using Astragon.Configuration.Shared;
using Astragon.Model.Dtos.Teacher;

namespace Astragon.Modules.Teacher.Application.Port;

public interface ITeacherOutPort : IBasePresenter<object>
{
    void GetAllAsync(IEnumerable<TeacherDto> data);
    void GetById(TeacherDto teacher);
    void Login(TeacherDto data);
}