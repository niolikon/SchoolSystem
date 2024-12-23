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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            throw new InvalidOperationException("DbContext not configured");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CourseModel>()
            .ToTable("Courses");

        modelBuilder.Entity<CourseModel>()
            .HasMany(course => course.Students)
            .WithMany(student => student.Courses)
            .UsingEntity("CoursesEnrolledStudents");

        modelBuilder.Entity<CourseModel>()
            .HasOne(course => course.Teacher)
            .WithMany(teacher => teacher.Courses)
            .HasForeignKey(course => course.TeacherId)
            .IsRequired(false);

        modelBuilder.Entity<StudentModel>()
            .ToTable("Students");

        modelBuilder.Entity<TeacherModel>()
            .ToTable("Teachers");
    }
}
