using Microsoft.EntityFrameworkCore;
using SchoolSystem.Core.Course;
using SchoolSystem.Core.Student;
using SchoolSystem.Core.Teacher;


namespace SchoolSystem.Infrastracture.Common;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<CourseModel> Courses => Set<CourseModel>();
    public DbSet<StudentModel> Students => Set<StudentModel>();
    public DbSet<TeacherModel> Teachers => Set<TeacherModel>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StudentModel>()
            .HasMany(student => student.Courses)
            .WithMany(course => course.Students)
            .UsingEntity("CoursesEnrolledStudents");
    }
}
