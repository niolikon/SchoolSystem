using SchoolSystem.Core.Teacher;


namespace SchoolSystem.Tests.Core.Teacher;

public static class TeacherTestData
{
    public static readonly TeacherModel TEACHER_MODEL_ASSISTANT = new()
    {
        Id = 1,
        FullName = "Sample Teacher",
        Position = "Assistant",
        Email = "sample.assistant.teacher@uni.ts"
    };

    public static readonly TeacherModel TEACHER_MODEL_ASSOCIATED = new()
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
            yield return new TestCaseData(TEACHER_MODEL_ASSISTANT);
            yield return new TestCaseData(TEACHER_MODEL_ASSOCIATED);
        }
    }

    public static readonly TeacherDto TEACHER_DTO_ASSISTANT = new()
    {
        Id = 1,
        FullName = "Sample Teacher",
        Position = "Assistant",
        Email = "sample.assistant.teacher@uni.ts"
    };

    public static readonly TeacherDto TEACHER_DTO_ASSOCIATED = new()
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
            yield return new TestCaseData(TEACHER_DTO_ASSISTANT);
            yield return new TestCaseData(TEACHER_DTO_ASSOCIATED);
        }
    }

    public static IEnumerable<TestCaseData> TeacherModelAndRelatedDtoTestCases
    {
        get
        {
            yield return new TestCaseData(TEACHER_MODEL_ASSISTANT, TEACHER_DTO_ASSISTANT);
            yield return new TestCaseData(TEACHER_MODEL_ASSOCIATED, TEACHER_DTO_ASSOCIATED);
        }
    }
}
