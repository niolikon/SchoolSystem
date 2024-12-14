using SchoolSystem.Core.Course;
using SchoolSystem.Core.Teacher;
using SchoolSystem.Tests.Core.Course;


namespace SchoolSystem.Tests.Core.Teacher;

public static class TeacherTestData
{
    public static readonly TeacherModel TEACHER_MODEL_1_ASSISTANT = new()
    {
        Id = 1,
        FullName = "Sample Teacher",
        Position = "Assistant",
        Email = "sample.assistant.teacher@uni.ts"
    };

    public static readonly TeacherModel TEACHER_MODEL_2_ASSOCIATED = new()
    {
        Id = 2,
        FullName = "Sample Teacher",
        Position = "Associated",
        Email = "sample.associated.teacher@uni.ts"
    };

    public static IEnumerable<TestCaseData> TeacherModelTestCases
    {
        get
        {
            yield return new TestCaseData(TEACHER_MODEL_1_ASSISTANT);
            yield return new TestCaseData(TEACHER_MODEL_2_ASSOCIATED);
        }
    }

    public static readonly TeacherDto TEACHER_DTO_1_ASSISTANT = new()
    {
        Id = 1,
        FullName = "Sample Teacher",
        Position = "Assistant",
        Email = "sample.assistant.teacher@uni.ts"
    };

    public static readonly TeacherDto TEACHER_DTO_2_ASSOCIATED = new()
    {
        Id = 2,
        FullName = "Sample Teacher",
        Position = "Associated",
        Email = "sample.associated.teacher@uni.ts"
    };

    public static IEnumerable<TestCaseData> TeacherDtoTestCases
    {
        get
        {
            yield return new TestCaseData(TEACHER_DTO_1_ASSISTANT);
            yield return new TestCaseData(TEACHER_DTO_2_ASSOCIATED);
        }
    }

    public static IEnumerable<TestCaseData> TeacherModelAndRelatedDtoTestCases
    {
        get
        {
            yield return new TestCaseData(TEACHER_MODEL_1_ASSISTANT, TEACHER_DTO_1_ASSISTANT);
            yield return new TestCaseData(TEACHER_MODEL_2_ASSOCIATED, TEACHER_DTO_2_ASSOCIATED);
        }
    }

    public static IEnumerable<CourseModel> TEACHER_MODEL_1_COURSES = new List<CourseModel>
    {
        CourseTestData.COURSE_MODEL_1_CALCULUS,
        CourseTestData.COURSE_MODEL_3_STATISTICS
    };

    public static IEnumerable<CourseModel> TEACHER_MODEL_2_COURSES = new List<CourseModel>
    {
        CourseTestData.COURSE_MODEL_2_ALGEBRA
    };
}
