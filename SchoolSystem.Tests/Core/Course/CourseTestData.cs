using SchoolSystem.Core.Course;
using SchoolSystem.Tests.Core.Teacher;


namespace SchoolSystem.Tests.Core.Course;

public static class CourseTestData
{
    public static readonly CourseModel COURSE_MODEL_CALCULUS = new()
    {
        Id = 1,
        Name = "Calculus",
        Credits = 10,
        TeacherId = TeacherTestData.TEACHER_MODEL_ASSISTANT.Id
    };

    public static readonly CourseModel COURSE_MODEL_ALGEBRA = new()
    {
        Id = 2,
        Name = "Linear Algebra",
        Credits = 6,
        TeacherId = TeacherTestData.TEACHER_MODEL_ASSOCIATED.Id
    };

    public static readonly CourseModel COURSE_MODEL_STATISTICS = new()
    {
        Id = 3,
        Name = "Probability and Statistics",
        Credits = 6,
        TeacherId = TeacherTestData.TEACHER_MODEL_ASSISTANT.Id
    };

    public static IEnumerable<TestCaseData> CourseModelTestCases
    {
        get
        {
            yield return new TestCaseData(COURSE_MODEL_CALCULUS);
            yield return new TestCaseData(COURSE_MODEL_ALGEBRA);
            yield return new TestCaseData(COURSE_MODEL_STATISTICS);
        }
    }

    public static readonly CourseDto COURSE_DTO_CALCULUS = new()
    {
        Id = 1,
        Name = "Calculus",
        Credits = 10,
        TeacherId = TeacherTestData.TEACHER_DTO_ASSISTANT.Id
    };

    public static readonly CourseDto COURSE_DTO_ALGEBRA = new()
    {
        Id = 2,
        Name = "Linear Algebra",
        Credits = 6,
        TeacherId = TeacherTestData.TEACHER_DTO_ASSOCIATED.Id
    };

    public static readonly CourseDto COURSE_DTO_STATISTICS = new()
    {
        Id = 3,
        Name = "Probability and Statistics",
        Credits = 6,
        TeacherId = TeacherTestData.TEACHER_DTO_ASSISTANT.Id
    };

    public static IEnumerable<TestCaseData> CourseDtoTestCases
    {
        get
        {
            yield return new TestCaseData(COURSE_DTO_CALCULUS);
            yield return new TestCaseData(COURSE_DTO_ALGEBRA);
            yield return new TestCaseData(COURSE_DTO_STATISTICS);
        }
    }

    public static IEnumerable<TestCaseData> CourseModelAndRelatedDtoTestCases
    {
        get
        {
            yield return new TestCaseData(COURSE_MODEL_CALCULUS, COURSE_DTO_CALCULUS);
            yield return new TestCaseData(COURSE_MODEL_ALGEBRA, COURSE_DTO_ALGEBRA);
            yield return new TestCaseData(COURSE_MODEL_STATISTICS, COURSE_DTO_STATISTICS);
        }
    }
}
