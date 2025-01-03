﻿using SchoolSystem.Core.Course;
using SchoolSystem.Core.Student;
using SchoolSystem.Core.Teacher;
using SchoolSystem.Core.Exceptions.Domain;
using SchoolSystem.Infrastracture.Common.BaseClasses;
using SchoolSystem.Infrastracture.Common;
using Microsoft.EntityFrameworkCore;

namespace SchoolSystem.Infrastracture.Student;

public class StudentRepository : BaseRepository<StudentModel, int>, IStudentRepository
{
    public StudentRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<bool> IsEmailInUse(string email)
    {
        IQueryable<StudentModel> students = _dbContext.Set<StudentModel>();
        IQueryable<TeacherModel> teachers = _dbContext.Set<TeacherModel>();

        return await Task.FromResult(
            students.Any(s => s.Email.Equals(email)) ||
            teachers.Any(t => t.Email.Equals(email))
        );
    }

    override public async Task<StudentModel> Create(StudentModel model)
    {
        if (await IsEmailInUse(model.Email))
        {
            throw new EmailAlreadyExistsDomainException($"Email {model.Email} already in use");
        }

        return await base.Create(model);
    }

    public async Task<IEnumerable<StudentModel>> FindStudentsByCourseId(int courseId)
    {
        IQueryable<StudentModel> students = _dbContext.Set<StudentModel>();
        IQueryable<CourseModel> courses = _dbContext.Set<CourseModel>();

        IQueryable<StudentModel> query =
            from s in students
            from c in courses
            where c.Id == courseId
            select s;

        try
        {
            return await query.ToListAsync();
        }
        catch (Exception ex)
        {
            throw new DatabaseOperationDomainException("Failed to find Students by course ID.", ex);
        }
    }

    public override async Task<IEnumerable<StudentModel>> GetAllWithDetails()
    {
        IQueryable<StudentModel> students = _dbContext.Set<StudentModel>();

        IQueryable<StudentModel> query = (
            from s in students
            select new StudentModel()
            {
                Id = s.Id,
                FullName = s.FullName,
                Email = s.Email,
                Courses = (from c in s.Courses
                           select new CourseModel()
                           {
                               Id = c.Id,
                               Credits = c.Credits,
                               Name = c.Name
                           }).ToList()
            });

        try
        {
            return await query.ToListAsync();
        }
        catch (Exception ex)
        {
            throw new DatabaseOperationDomainException("Failed to get Students with details.", ex);
        }
    }

    public override async Task<StudentModel> GetByIdWithDetails(int id)
    {
        IQueryable<StudentModel> students = _dbContext.Set<StudentModel>();

        IQueryable<StudentModel> query = (
            from s in students
            where s.Id == id
            select new StudentModel()
            {
                Id = s.Id,
                FullName = s.FullName,
                Email = s.Email,
                Courses = (from c in s.Courses
                           select new CourseModel()
                           {
                               Id = c.Id,
                               Credits = c.Credits,
                               Name = c.Name
                           }).ToList()
            });

        try
        {
            return await query.FirstOrDefaultAsync() ??
                throw new EntityNotFoundDomainException(typeof(StudentModel).ToString(), id);
        }
        catch (EntityNotFoundDomainException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new DatabaseOperationDomainException("Failed to retrieve Student details by ID.", ex);
        }
    }
}
