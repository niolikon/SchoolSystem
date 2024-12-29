using SchoolSystem.Core.Teacher;


namespace SchoolSystem.Tests.Core.Teacher;

public record TeacherTestData
{
    public static TeacherModel TEACHER_MODEL_1_ASSISTANT => new()
    {
        Id = 1,
        FullName = "Sample Teacher",
        Position = AcademicPosition.AssistantProfessor,
        Email = "sample.assistant.teacher@uni.ts"
    };

    public static TeacherModel TEACHER_MODEL_2_ASSOCIATED => new()
    {
        Id = 2,
        FullName = "Sample Teacher",
        Position = AcademicPosition.AssociateProfessor,
        Email = "sample.associated.teacher@uni.ts"
    };

    public static readonly IEnumerable<object[]> TeacherModelTestCases =
    [
        [TEACHER_MODEL_1_ASSISTANT],
        [TEACHER_MODEL_2_ASSOCIATED]
    ];

    public static TeacherDto TEACHER_DTO_1_ASSISTANT => new()
    {
        Id = 1,
        FullName = "Sample Teacher",
        Position = "AssistantProfessor",
        Email = "sample.assistant.teacher@uni.ts"
    };

    public static TeacherDto TEACHER_DTO_2_ASSOCIATED => new()
    {
        Id = 2,
        FullName = "Sample Teacher",
        Position = "AssociatedProfessor",
        Email = "sample.associated.teacher@uni.ts"
    };

    public static readonly IEnumerable<object[]> TeacherDtoTestCases =
    [
        [TEACHER_DTO_1_ASSISTANT],
        [TEACHER_DTO_2_ASSOCIATED]
    ];

    public static readonly IEnumerable<object[]> TeacherModelAndRelatedDtoTestCases =
    [
        [TEACHER_MODEL_1_ASSISTANT, TEACHER_DTO_1_ASSISTANT],
        [TEACHER_MODEL_2_ASSOCIATED, TEACHER_DTO_2_ASSOCIATED]
    ];
}
