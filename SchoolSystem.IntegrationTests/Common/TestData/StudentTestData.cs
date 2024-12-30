using SchoolSystem.Core.Student;


namespace SchoolSystem.IntegrationTests.Common.TestData;

public static class StudentTestData
{
    public static StudentModel STUDENT_MODEL_1 => new()
    {
        FullName = "Sample Student 1",
        Email = "sample.student1@uni.ts"
    };

    public static StudentModel STUDENT_MODEL_2 => new()
    {
        FullName = "Sample Student 2",
        Email = "sample.student2@uni.ts"
    };
}
