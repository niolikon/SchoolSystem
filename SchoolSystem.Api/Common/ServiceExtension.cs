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
        services.AddSingleton<IBaseMapper<CourseModel, CourseDetailsDto>>(new BaseMapper<CourseModel, CourseDetailsDto>(mapper));
        services.AddSingleton<IBaseMapper<CourseDetailsDto, CourseModel>>(new BaseMapper<CourseDetailsDto, CourseModel>(mapper));
        services.AddSingleton<IBaseMapper<CourseCreateDto, CourseModel>>(new BaseMapper<CourseCreateDto, CourseModel>(mapper));
        services.AddSingleton<IBaseMapper<CourseUpdateDto, CourseModel>>(new BaseMapper<CourseUpdateDto, CourseModel>(mapper));

        services.AddSingleton<IBaseMapper<StudentModel, StudentDetailsDto>>(new BaseMapper<StudentModel, StudentDetailsDto>(mapper));
        services.AddSingleton<IBaseMapper<StudentDetailsDto, StudentModel>>(new BaseMapper<StudentDetailsDto, StudentModel>(mapper));
        services.AddSingleton<IBaseMapper<StudentCreateDto, StudentModel>>(new BaseMapper<StudentCreateDto, StudentModel>(mapper));
        services.AddSingleton<IBaseMapper<StudentUpdateDto, StudentModel>>(new BaseMapper<StudentUpdateDto, StudentModel>(mapper));

        services.AddSingleton<IBaseMapper<TeacherModel, TeacherDetailsDto>>(new BaseMapper<TeacherModel, TeacherDetailsDto>(mapper));
        services.AddSingleton<IBaseMapper<TeacherDetailsDto, TeacherModel>>(new BaseMapper<TeacherDetailsDto, TeacherModel>(mapper));
        services.AddSingleton<IBaseMapper<TeacherCreateDto, TeacherModel>>(new BaseMapper<TeacherCreateDto, TeacherModel>(mapper));
        services.AddSingleton<IBaseMapper<TeacherUpdateDto, TeacherModel>>(new BaseMapper<TeacherUpdateDto, TeacherModel>(mapper));
        #endregion

        #region Services
        services.AddScoped<ICourseService, CourseService>();
        services.AddScoped<IStudentService, StudentService>();
        services.AddScoped<ITeacherService, TeacherService>();
        #endregion

        return services;
    }
}
