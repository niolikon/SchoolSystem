using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Moq;
using SchoolSystem.Core.Course;
using SchoolSystem.Infrastracture.Common;
using SchoolSystem.Infrastracture.Course;
using SchoolSystem.Tests.Core.Course;
using SchoolSystem.Core.Teacher;

namespace SchoolSystem.Tests.Infrastructure.Course;

public class CourseRepositoryTest
{
    private Mock<ApplicationDbContext> _dbContextMock;
    private Mock<DbSet<CourseModel>> _dbSetCourseMock;
    private CourseRepository _courseRepository;

    [SetUp]
    public void Setup()
    {
        _dbContextMock = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
        _dbSetCourseMock = new Mock<DbSet<CourseModel>>();
        _dbContextMock.Setup(db => db.Set<CourseModel>())
            .Returns(_dbSetCourseMock.Object);

        _courseRepository = new CourseRepository(_dbContextMock.Object);

    }

    [Test]
    public async Task AddAsync_ValidProduct_ReturnsAddedProduct()
    {
        var courseToCreate = CourseTestData.COURSE_MODEL_CALCULUS;

        var courseCreated = await _courseRepository.Create(courseToCreate);

        _dbSetCourseMock.Verify(dbSet => dbSet.AddAsync(courseToCreate, default), Times.Once);
        _dbContextMock.Verify(dbSet => dbSet.SaveChangesAsync(default), Times.Once);
        Assert.That(courseCreated, Is.EqualTo(courseToCreate));
    }
}
