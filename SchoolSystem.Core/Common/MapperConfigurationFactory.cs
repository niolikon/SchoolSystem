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
            cfg.CreateMap<CourseModel, CourseDetailsDto>()
                .ForMember(dest => dest.Teacher, opt => opt.MapFrom(src => new TeacherDetailsDto
                {
                    Id = src.Teacher.Id,
                    FullName = src.Teacher.FullName,
                    Position = src.Teacher.Position.ToString(),
                    Email = src.Teacher.Email,
                    Courses = null
                }))
                .ForMember(dest => dest.Students, opt => opt.MapFrom(src => src.Students.Select(student => new StudentDetailsDto
                {
                    Id = student.Id,
                    FullName = student.FullName,
                    Email = student.Email,
                    Courses = null
                })));
            cfg.CreateMap<CourseCreateDto, CourseModel>();
            cfg.CreateMap<CourseUpdateDto, CourseModel>();

            cfg.CreateMap<StudentModel, StudentDetailsDto>()
                .ForMember(dest => dest.Courses, opt => opt.MapFrom(src => src.Courses.Select(course => new CourseDetailsDto
                {
                    Id = course.Id,
                    Name = course.Name,
                    Credits = course.Credits,
                    TeacherId = course.TeacherId
                })));
            cfg.CreateMap<StudentCreateDto, StudentModel>();
            cfg.CreateMap<StudentUpdateDto, StudentModel>();

            cfg.CreateMap<TeacherModel, TeacherDetailsDto>()
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position.ToString()))
                .ForMember(dest => dest.Courses, opt => opt.MapFrom(src => src.Courses.Select(course => new CourseDetailsDto
                {
                    Id = course.Id,
                    Name = course.Name,
                    Credits = course.Credits,
                    TeacherId = course.TeacherId
                })));
            cfg.CreateMap<TeacherCreateDto, TeacherModel>()
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => Enum.Parse<AcademicPosition>(src.Position, true)));
            cfg.CreateMap<TeacherUpdateDto, TeacherModel>()
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => Enum.Parse<AcademicPosition>(src.Position, true)));
        });

        return configuration.CreateMapper();
    }
}
