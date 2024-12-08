using Moq;
using SchoolSystem.Core.Base.BaseInterfaces;
using SchoolSystem.Core.Course;
using SchoolSystem.Core.Student;
using SchoolSystem.Tests.Core.Course;

namespace SchoolSystem.Tests.Core.Student
{
    [TestFixture]
    public class StudentServiceTests
    {
        private Mock<IBaseMapper<StudentModel, StudentDto>> _studentDtoMapperMock;
        private Mock<IBaseMapper<StudentDto, StudentModel>> _studentModelMapperMock;
        private Mock<IStudentRepository> _studentRepositoryMock;
        private Mock<IBaseMapper<CourseModel, CourseDto>> _courseDtoMapperMock;
        private Mock<ICourseRepository> _courseRepositoryMock;
        private StudentService studentService;

        [SetUp]
        public void Setup()
        {
            _studentDtoMapperMock = new Mock<IBaseMapper<StudentModel, StudentDto>>();
            _studentModelMapperMock = new Mock<IBaseMapper<StudentDto, StudentModel>>();
            _studentRepositoryMock = new Mock<IStudentRepository>();
            _courseDtoMapperMock = new Mock<IBaseMapper<CourseModel, CourseDto>>();
            _courseRepositoryMock = new Mock<ICourseRepository>();

            studentService = new StudentService(
                _studentDtoMapperMock.Object,
                _studentModelMapperMock.Object,
                _studentRepositoryMock.Object,
                _courseDtoMapperMock.Object,
                _courseRepositoryMock.Object);
        }

        [Test]
        public async Task GetAll_ReturnAll()
        {
            IEnumerable<StudentModel> studentsInDb = new List<StudentModel>
            {
                    StudentTestData.STUDENT_MODEL_1,
                    StudentTestData.STUDENT_MODEL_2
            };
            List<StudentDto> studentsAsDto = new List<StudentDto>
            {
                StudentTestData.STUDENT_DTO_1,
                StudentTestData.STUDENT_DTO_2
            };

            _studentRepositoryMock.Setup(repo => repo.GetAll())
                .ReturnsAsync(studentsInDb);

            _studentDtoMapperMock.Setup(mapper => mapper.MapList(studentsInDb))
                .Returns(studentsAsDto);

            IEnumerable<StudentDto> studentsReturned = await studentService.GetAll();

            Assert.That(studentsReturned, Is.EquivalentTo(studentsAsDto));
        }

        [Test]
        public async Task GetSingle_ShouldReturnStudentWithCourses()
        {
            var studentModel = StudentTestData.STUDENT_MODEL_2;
            var studentAsDto = StudentTestData.STUDENT_DTO_2;
            var coursesEnrolledModel = new List<CourseModel>
            {
                CourseTestData.COURSE_MODEL_ALGEBRA,
                CourseTestData.COURSE_MODEL_STATISTICS
            };
            var coursesEnrolledAsDto = new List<CourseDto>
            {
                CourseTestData.COURSE_DTO_ALGEBRA,
                CourseTestData.COURSE_DTO_STATISTICS
            };

            _studentRepositoryMock
                .Setup(repo => repo.GetById(studentModel.Id))
                .ReturnsAsync(studentModel);

            _studentDtoMapperMock
                .Setup(mapper => mapper.MapInstance(studentModel))
                .Returns(studentAsDto);

            _courseRepositoryMock
                .Setup(repo => repo.FindCoursesByStudentId(studentModel.Id))
                .ReturnsAsync(coursesEnrolledModel);

            _courseDtoMapperMock
                .Setup(mapper => mapper.MapList(coursesEnrolledModel))
                .Returns(coursesEnrolledAsDto);

            StudentDto studentReturned = await studentService.GetSingle(studentModel.Id);

            Assert.Multiple(() =>
            {
                Assert.That(studentReturned, Is.EqualTo(studentAsDto));
                Assert.That(studentReturned.EnrolledCourses, Is.EquivalentTo(coursesEnrolledAsDto));
            });
        }

        [Test]
        public async Task Create_ShouldReturnCreatedStudentDto()
        {
            var studentDto = StudentTestData.STUDENT_DTO_2;
            var studentAsModel = StudentTestData.STUDENT_MODEL_2;
            StudentDto studentDtoCreated = new()
            {
                Id = 2345678,
                FullName = studentDto.FullName,
                Email = studentDto.Email,
            };

            _studentModelMapperMock
                .Setup(mapper => mapper.MapInstance(studentDto))
                .Returns(studentAsModel);

            _studentRepositoryMock
                .Setup(repo => repo.Create(It.IsAny<StudentModel>()))
                .ReturnsAsync(studentAsModel);

            _studentDtoMapperMock
                .Setup(mapper => mapper.MapInstance(studentAsModel))
                .Returns(studentDtoCreated);

            StudentDto studentReturned = await studentService.Create(studentDto);

            Assert.Multiple(() =>
            {
                Assert.That(studentReturned.FullName, Is.EqualTo(studentDtoCreated.FullName));
                Assert.That(studentReturned.Email, Is.EqualTo(studentDtoCreated.Email));
            });
        }
    }
}
