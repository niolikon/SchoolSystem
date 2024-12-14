using Microsoft.EntityFrameworkCore;
using SchoolSystem.Core.Course;
using SchoolSystem.Core.CourseEnrollment;
using SchoolSystem.Core.Student;
using SchoolSystem.Core.Teacher;


namespace SchoolSystem.Infrastracture.Common;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<CourseModel> Courses => Set<CourseModel>();
    public DbSet<CourseEnrollmentModel> CourseEnrollment => Set<CourseEnrollmentModel>();
    public DbSet<StudentModel> Students => Set<StudentModel>();
    public DbSet<TeacherModel> Teachers => Set<TeacherModel>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CourseEnrollmentModel>()
            .HasKey(sc => new { sc.StudentId, sc.CourseId });

        modelBuilder.Entity<CourseEnrollmentModel>()
            .HasOne(sc => sc.Student)
            .WithMany(s => s.Enrollments)
            .HasForeignKey(sc => sc.StudentId);

        modelBuilder.Entity<CourseEnrollmentModel>()
            .HasOne(sc => sc.Course)
            .WithMany(c => c.Enrollments)
            .HasForeignKey(sc => sc.CourseId);
    }
}
