using SchoolSystem.Core.Course;
using SchoolSystem.Core.CourseEnrollment;
using SchoolSystem.Core.Student;
using SchoolSystem.Tests.Core.Course;


namespace SchoolSystem.Tests.Core.Student;

public static class StudentTestData
{
    public static readonly StudentModel STUDENT_MODEL_1 = new()
    {
        Id = 1,
        FullName = "Sample Student 1",
        Email = "sample.student1@uni.ts"
    };

    public static readonly StudentModel STUDENT_MODEL_2 = new()
    {
        Id = 2,
        FullName = "Sample Student 2",
        Email = "sample.student2@uni.ts"
    };

    public static IEnumerable<TestCaseData> StudentModelTestCases
    {
        get
        {
            yield return new TestCaseData(STUDENT_MODEL_1);
            yield return new TestCaseData(STUDENT_MODEL_2);
        }
    }

    public static readonly StudentDto STUDENT_DTO_1 = new()
    {
        Id = 1,
        FullName = "Sample Student 1",
        Email = "sample.student1@uni.ts"
    };

    public static readonly StudentDto STUDENT_DTO_2 = new()
    {
        Id = 2,
        FullName = "Sample Student 2",
        Email = "sample.student2@uni.ts"
    };

    public static IEnumerable<TestCaseData> StudentDtoTestCases
    {
        get
        {
            yield return new TestCaseData(STUDENT_DTO_1);
            yield return new TestCaseData(STUDENT_DTO_2);
        }
    }

    public static IEnumerable<TestCaseData> StudentModelAndRelatedDtoTestCases
    {
        get
        {
            yield return new TestCaseData(STUDENT_MODEL_1, STUDENT_DTO_1);
            yield return new TestCaseData(STUDENT_MODEL_2, STUDENT_DTO_2);
        }
    }

    public static IEnumerable<CourseEnrollmentModel> STUDENT_MODEL_1_ENROLLMENTS = new List<CourseEnrollmentModel>
    {
        new CourseEnrollmentModel { StudentId = STUDENT_MODEL_1.Id, CourseId = CourseTestData.COURSE_MODEL_2_ALGEBRA.Id },
        new CourseEnrollmentModel { StudentId = STUDENT_MODEL_1.Id, CourseId = CourseTestData.COURSE_MODEL_1_CALCULUS.Id }
    };

    public static IEnumerable<CourseModel> STUDENT_MODEL_1_COURSES = new List<CourseModel>
    {
        CourseTestData.COURSE_MODEL_2_ALGEBRA,
        CourseTestData.COURSE_MODEL_1_CALCULUS
    };


    public static IEnumerable<CourseEnrollmentModel> STUDENT_MODEL_2_ENROLLMENTS = new List<CourseEnrollmentModel>
    {
        new CourseEnrollmentModel { StudentId = STUDENT_MODEL_2.Id, CourseId = CourseTestData.COURSE_MODEL_3_STATISTICS.Id }
    };

    public static IEnumerable<CourseModel> STUDENT_MODEL_2_COURSES = new List<CourseModel>
    {
        CourseTestData.COURSE_MODEL_3_STATISTICS
    };
}
