using SchoolSystem.Core.Common.BaseInterfaces;

namespace SchoolSystem.Core.Teacher;

public interface ITeacherService : IBaseService<TeacherDetailsDto, TeacherCreateDto, TeacherUpdateDto>
{
}