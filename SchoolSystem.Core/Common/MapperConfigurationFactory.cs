using AutoMapper;
using SchoolSystem.Core.Course;
using SchoolSystem.Core.Student;
using SchoolSystem.Core.Teacher;

namespace SchoolSystem.Core.Common;


public static class MapperConfigurationFactory
{
    public static IMapper CreateMapper()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<CourseModel, CourseDto>()
                .ForMember(dest => dest.Teacher, opt => opt.MapFrom(src => new TeacherDto
                {
                    Id = src.Teacher.Id,
                    FullName = src.Teacher.FullName,
                    Position = src.Teacher.Position.ToString(),
                    Email = src.Teacher.Email,
                    Courses = null
                }))
                .ForMember(dest => dest.Students, opt => opt.MapFrom(src => src.Students.Select(student => new StudentDto
                {
                    Id = student.Id,
                    FullName = student.FullName,
                    Email = student.Email,
                    Courses = null
                })));
            cfg.CreateMap<CourseDto, CourseModel>();

            cfg.CreateMap<StudentModel, StudentDto>()
                .ForMember(dest => dest.Courses, opt => opt.MapFrom(src => src.Courses.Select(course => new CourseDto
                {
                    Id = course.Id,
                    Name = course.Name,
                    Credits = course.Credits,
                    TeacherId = course.TeacherId
                })));
            cfg.CreateMap<StudentDto, StudentModel>();

            cfg.CreateMap<TeacherModel, TeacherDto>()
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position.ToString()))
                .ForMember(dest => dest.Courses, opt => opt.MapFrom(src => src.Courses.Select(course => new CourseDto
                {
                    Id = course.Id,
                    Name = course.Name,
                    Credits = course.Credits,
                    TeacherId = course.TeacherId
                })));
            cfg.CreateMap<TeacherDto, TeacherModel>()
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => Enum.Parse<AcademicPosition>(src.Position, true)));
        });

        return configuration.CreateMapper();
    }
}
