using SchoolSystem.Core.Teacher;


namespace SchoolSystem.IntegrationTests.Infrastructure.Teacher;

public static class TeacherTestData
{
    private static readonly TeacherModel _TEACHER_MODEL_1 = new()
    {
        FullName = "Sample Teacher",
        Position = "Assistant",
        Email = "sample.assistant.teacher@uni.ts"
    };

    private static readonly TeacherModel _TEACHER_MODEL_2  = new()
    {
        FullName = "Sample Teacher",
        Position = "Associated",
        Email = "sample.associated.teacher@uni.ts"
    };

    public static TeacherModel TEACHER_MODEL_1 => _TEACHER_MODEL_1.Clone;
    public static TeacherModel TEACHER_MODEL_2 => _TEACHER_MODEL_2.Clone;
}
