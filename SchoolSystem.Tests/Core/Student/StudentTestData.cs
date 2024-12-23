using SchoolSystem.Core.Student;


namespace SchoolSystem.Tests.Core.Student;

public record StudentTestData
{
    public static StudentModel STUDENT_MODEL_1 => new()
    {
        Id = 1,
        FullName = "Sample Student 1",
        Email = "sample.student1@uni.ts"
    };

    public static StudentModel STUDENT_MODEL_2 => new()
    {
        Id = 2,
        FullName = "Sample Student 2",
        Email = "sample.student2@uni.ts"
    };

    public static readonly IEnumerable<object[]> StudentModelTestCases =
    [
        [STUDENT_MODEL_1],
        [STUDENT_MODEL_2]
    ];

    public static StudentDto STUDENT_DTO_1 => new()
    {
        Id = 1,
        FullName = "Sample Student 1",
        Email = "sample.student1@uni.ts"
    };

    public static StudentDto STUDENT_DTO_2 => new()
    {
        Id = 2,
        FullName = "Sample Student 2",
        Email = "sample.student2@uni.ts"
    };

    public static readonly IEnumerable<object[]> StudentDtoTestCases =
    [
        [STUDENT_DTO_1],
        [STUDENT_DTO_1]
    ];

    public static readonly IEnumerable<object[]> StudentModelAndRelatedDtoTestCases =
    [
        [STUDENT_MODEL_1, STUDENT_DTO_1],
        [STUDENT_MODEL_2, STUDENT_DTO_2]
    ];
}
