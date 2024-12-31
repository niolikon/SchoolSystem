using SchoolSystem.Core.Teacher;
using SchoolSystem.Tests.Core.Course;

namespace SchoolSystem.Tests.Core.Teacher;

public static class TeacherTestData
{
    #region TeacherModel
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
    #endregion

    #region TeacherDetails
    public static TeacherDetailsDto TEACHER_DETAILS_1_ASSISTANT => new()
    {
        Id = 1,
        FullName = "Sample Teacher",
        Position = AcademicPosition.AssistantProfessor.ToString(),
        Email = "sample.assistant.teacher@uni.ts",
        Courses = [CourseTestData.COURSE_DETAILS_1_CALCULUS, CourseTestData.COURSE_DETAILS_3_STATISTICS]
    };

    public static TeacherDetailsDto TEACHER_DETAILS_2_ASSOCIATED => new()
    {
        Id = 2,
        FullName = "Sample Teacher",
        Position = AcademicPosition.AssociateProfessor.ToString(),
        Email = "sample.associated.teacher@uni.ts",
        Courses = [CourseTestData.COURSE_DETAILS_2_ALGEBRA]
    };
    #endregion

    #region TeacherCreateDto
    public static TeacherCreateDto TEACHER_CREATE_DTO_1_ASSISTANT => new()
    {
        FullName = "Sample Teacher",
        Position = AcademicPosition.AssistantProfessor.ToString(),
        Email = "sample.assistant.teacher@uni.ts"
    };

    public static TeacherCreateDto TEACHER_CREATE_DTO_2_ASSOCIATED => new()
    {
        FullName = "Sample Teacher",
        Position = AcademicPosition.AssociateProfessor.ToString(),
        Email = "sample.associated.teacher@uni.ts"
    };

    public static readonly IEnumerable<object[]> TeacherCreateDtoTestCases =
    [
        [TEACHER_CREATE_DTO_1_ASSISTANT],
        [TEACHER_CREATE_DTO_2_ASSOCIATED]
    ];
    #endregion

    #region TeacherUpdateDto
    public static TeacherUpdateDto TEACHER_UPDATE_DTO_1_ASSISTANT => new()
    {
        Id = 1,
        FullName = "Sample Teacher",
        Position = AcademicPosition.AssistantProfessor.ToString(),
        Email = "sample.assistant.teacher@uni.ts"
    };

    public static TeacherUpdateDto TEACHER_UPDATE_DTO_2_ASSOCIATED => new()
    {
        Id = 2,
        FullName = "Sample Teacher",
        Position = AcademicPosition.AssociateProfessor.ToString(),
        Email = "sample.associated.teacher@uni.ts"
    };

    public static readonly IEnumerable<object[]> TeacherUpdateDtoTestCases =
    [
        [TEACHER_UPDATE_DTO_1_ASSISTANT],
        [TEACHER_UPDATE_DTO_2_ASSOCIATED]
    ];
    #endregion
}
