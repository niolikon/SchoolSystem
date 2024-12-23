using SchoolSystem.Infrastracture.Common.BaseClasses;
using SchoolSystem.Infrastracture.Common;
using SchoolSystem.Core.Course;
using SchoolSystem.Core.Teacher;
using SchoolSystem.Core.Exceptions.Domain;
using Microsoft.EntityFrameworkCore;

namespace SchoolSystem.Infrastracture.Teacher;


public class TeacherRepository : BaseRepository<TeacherModel, int>, ITeacherRepository
{
    public TeacherRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public override async Task<IEnumerable<TeacherModel>> GetAllWithDetails()
    {
        IQueryable<TeacherModel> teachers = _dbContext.Set<TeacherModel>();

        IQueryable<TeacherModel> query = (
            from t in teachers
            select new TeacherModel()
            {
                Id = t.Id,
                FullName = t.FullName,
                Email = t.Email,
                Position = t.Position,
                Courses = (from c in t.Courses
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
            throw new DatabaseOperationDomainException("Failed to get Teacher with details.", ex);
        }
    }

    public override async Task<TeacherModel> GetByIdWithDetails(int id)
    {
        IQueryable<TeacherModel> teachers = _dbContext.Set<TeacherModel>();

        IQueryable<TeacherModel> query = (
            from t in teachers
            where t.Id == id
            select new TeacherModel()
            {
                Id = t.Id,
                FullName = t.FullName,
                Email = t.Email,
                Position = t.Position,
                Courses = (from c in t.Courses
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
                throw new EntityNotFoundDomainException(typeof(TeacherModel).ToString(), id);
        }
        catch (EntityNotFoundDomainException ex)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new DatabaseOperationDomainException("Failed to retrieve Teacher details by ID.", ex);
        }
    }
}
