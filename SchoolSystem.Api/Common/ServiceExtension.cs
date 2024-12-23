using AutoMapper;
using SchoolSystem.Core.Common.BaseClasses;
using SchoolSystem.Core.Common.BaseInterfaces;
using SchoolSystem.Core.Course;
using SchoolSystem.Infrastracture.Course;
using SchoolSystem.Core.Student;
using SchoolSystem.Infrastracture.Student;
using SchoolSystem.Core.Teacher;
using SchoolSystem.Infrastracture.Teacher;
using Microsoft.Extensions.DependencyInjection;
using SchoolSystem.Core.Common;

namespace SchoolSystem.Api.Common;

public static class ServiceExtension
{
    public static IServiceCollection RegisterService(this IServiceCollection services)
    {
        #region Repositories
        services.AddTransient<ICourseRepository, CourseRepository>();
        services.AddTransient<IStudentRepository, StudentRepository>();
        services.AddTransient<ITeacherRepository, TeacherRepository>();
        #endregion

        #region Mapper
        IMapper mapper = MapperConfigurationFactory.CreateMapper();
        services.AddSingleton<IMapper>(mapper);
        services.AddSingleton<IBaseMapper<CourseModel, CourseDto>>(new BaseMapper<CourseModel, CourseDto>(mapper));
        services.AddSingleton<IBaseMapper<CourseDto, CourseModel>>(new BaseMapper<CourseDto, CourseModel>(mapper));

        services.AddSingleton<IBaseMapper<StudentModel, StudentDto>>(new BaseMapper<StudentModel, StudentDto>(mapper));
        services.AddSingleton<IBaseMapper<StudentDto, StudentModel>>(new BaseMapper<StudentDto, StudentModel>(mapper));

        services.AddSingleton<IBaseMapper<TeacherModel, TeacherDto>>(new BaseMapper<TeacherModel, TeacherDto>(mapper));
        services.AddSingleton<IBaseMapper<TeacherDto, TeacherModel>>(new BaseMapper<TeacherDto, TeacherModel>(mapper));
        #endregion

        #region Services
        services.AddScoped<ICourseService, CourseService>();
        services.AddScoped<IStudentService, StudentService>();
        services.AddScoped<ITeacherService, TeacherService>();
        #endregion

        return services;
    }
}
