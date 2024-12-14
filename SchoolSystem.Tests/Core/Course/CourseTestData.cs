using SchoolSystem.Core.Course;
using SchoolSystem.Tests.Core.Teacher;


namespace SchoolSystem.Tests.Core.Course;

public static class CourseTestData
{
    public static readonly CourseModel COURSE_MODEL_1_CALCULUS = new()
    {
        Id = 1,
        Name = "Calculus",
        Credits = 10,
        TeacherId = TeacherTestData.TEACHER_MODEL_1_ASSISTANT.Id
    };

    public static readonly CourseModel COURSE_MODEL_2_ALGEBRA = new()
    {
        Id = 2,
        Name = "Linear Algebra",
        Credits = 6,
        TeacherId = TeacherTestData.TEACHER_MODEL_2_ASSOCIATED.Id
    };

    public static readonly CourseModel COURSE_MODEL_3_STATISTICS = new()
    {
        Id = 3,
        Name = "Probability and Statistics",
        Credits = 6,
        TeacherId = TeacherTestData.TEACHER_MODEL_1_ASSISTANT.Id
    };

    public static IEnumerable<TestCaseData> CourseModelTestCases
    {
        get
        {
            yield return new TestCaseData(COURSE_MODEL_1_CALCULUS);
            yield return new TestCaseData(COURSE_MODEL_2_ALGEBRA);
            yield return new TestCaseData(COURSE_MODEL_3_STATISTICS);
        }
    }

    public static readonly CourseDto COURSE_DTO_1_CALCULUS = new()
    {
        Id = 1,
        Name = "Calculus",
        Credits = 10,
        TeacherId = TeacherTestData.TEACHER_DTO_1_ASSISTANT.Id
    };

    public static readonly CourseDto COURSE_DTO_2_ALGEBRA = new()
    {
        Id = 2,
        Name = "Linear Algebra",
        Credits = 6,
        TeacherId = TeacherTestData.TEACHER_DTO_2_ASSOCIATED.Id
    };

    public static readonly CourseDto COURSE_DTO_3_STATISTICS = new()
    {
        Id = 3,
        Name = "Probability and Statistics",
        Credits = 6,
        TeacherId = TeacherTestData.TEACHER_DTO_1_ASSISTANT.Id
    };

    public static IEnumerable<TestCaseData> CourseDtoTestCases
    {
        get
        {
            yield return new TestCaseData(COURSE_DTO_1_CALCULUS);
            yield return new TestCaseData(COURSE_DTO_2_ALGEBRA);
            yield return new TestCaseData(COURSE_DTO_3_STATISTICS);
        }
    }

    public static IEnumerable<TestCaseData> CourseModelAndRelatedDtoTestCases
    {
        get
        {
            yield return new TestCaseData(COURSE_MODEL_1_CALCULUS, COURSE_DTO_1_CALCULUS);
            yield return new TestCaseData(COURSE_MODEL_2_ALGEBRA, COURSE_DTO_2_ALGEBRA);
            yield return new TestCaseData(COURSE_MODEL_3_STATISTICS, COURSE_DTO_3_STATISTICS);
        }
    }
}
