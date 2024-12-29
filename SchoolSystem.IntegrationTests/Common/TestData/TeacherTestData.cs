using SchoolSystem.Core.Teacher;


namespace SchoolSystem.IntegrationTests.Common.TestData;

public static class TeacherTestData
{
    public static TeacherModel TEACHER_MODEL_1 => new()
    {
        FullName = "Sample Teacher",
        Position = "Assistant",
        Email = "sample.assistant.teacher@uni.ts"
    };

    public static TeacherModel TEACHER_MODEL_2 => new()
    {
        FullName = "Sample Teacher",
        Position = "Associated",
        Email = "sample.associated.teacher@uni.ts"
    };
}
