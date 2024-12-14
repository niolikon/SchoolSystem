using SchoolSystem.Core.Student;


namespace SchoolSystem.IntegrationTests.Infrastructure.Student;

public static class StudentTestData
{
    private static readonly StudentModel _STUDENT_MODEL_1 = new()
    {
        FullName = "Sample Student 1",
        Email = "sample.student1@uni.ts"
    };

    private static readonly StudentModel _STUDENT_MODEL_2 = new()
    {
        FullName = "Sample Student 2",
        Email = "sample.student2@uni.ts"
    };

    public static StudentModel STUDENT_MODEL_1 => _STUDENT_MODEL_1.Clone;
    public static StudentModel STUDENT_MODEL_2 => _STUDENT_MODEL_2.Clone;
}
