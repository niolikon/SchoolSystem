using SchoolSystem.Core.Course;
using SchoolSystem.Tests.Core.Teacher;

namespace SchoolSystem.Tests.Core.Course;

public static class CourseTestData
{
    #region CourseModel
    public static CourseModel COURSE_MODEL_1_CALCULUS => new()
    {
        Id = 1,
        Name = "Calculus",
        Credits = 10,
        TeacherId = TeacherTestData.TEACHER_MODEL_1_ASSISTANT.Id
    };

    public static CourseModel COURSE_MODEL_2_ALGEBRA => new()
    {
        Id = 2,
        Name = "Linear Algebra",
        Credits = 6,
        TeacherId = TeacherTestData.TEACHER_MODEL_2_ASSOCIATED.Id
    };

    public static CourseModel COURSE_MODEL_3_STATISTICS => new()
    {
        Id = 3,
        Name = "Probability and Statistics",
        Credits = 6,
        TeacherId = TeacherTestData.TEACHER_MODEL_1_ASSISTANT.Id
    };

    public static readonly IEnumerable<object[]> CourseModelTestCases =
    [
        [COURSE_MODEL_1_CALCULUS],
        [COURSE_MODEL_2_ALGEBRA],
        [COURSE_MODEL_3_STATISTICS]
    ];
    #endregion


    #region CourseDetails
    public static CourseDetailsDto COURSE_DETAILS_1_CALCULUS => new()
    {
        Id = 1,
        Name = "Calculus",
        Credits = 10,
        TeacherId = TeacherTestData.TEACHER_MODEL_1_ASSISTANT.Id
    };

    public static CourseDetailsDto COURSE_DETAILS_2_ALGEBRA => new()
    {
        Id = 2,
        Name = "Linear Algebra",
        Credits = 6,
        TeacherId = TeacherTestData.TEACHER_MODEL_2_ASSOCIATED.Id
    };

    public static CourseDetailsDto COURSE_DETAILS_3_STATISTICS => new()
    {
        Id = 3,
        Name = "Probability and Statistics",
        Credits = 6,
        TeacherId = TeacherTestData.TEACHER_MODEL_1_ASSISTANT.Id
    };
    #endregion

    #region CourseCreateDto
    public static CourseCreateDto COURSE_CREATE_DTO_1_CALCULUS => new()
    {
        Name = "Calculus",
        Credits = 10,
        TeacherId = TeacherTestData.TEACHER_MODEL_1_ASSISTANT.Id
    };

    public static CourseCreateDto COURSE_CREATE_DTO_2_ALGEBRA => new()
    {
        Name = "Linear Algebra",
        Credits = 6,
        TeacherId = TeacherTestData.TEACHER_MODEL_2_ASSOCIATED.Id
    };

    public static CourseCreateDto COURSE_CREATE_DTO_3_STATISTICS => new()
    {
        Name = "Probability and Statistics",
        Credits = 6,
        TeacherId = TeacherTestData.TEACHER_MODEL_1_ASSISTANT.Id
    };

    public static readonly IEnumerable<object[]> CourseCreateDtoTestCases =
    [
        [COURSE_CREATE_DTO_1_CALCULUS],
        [COURSE_CREATE_DTO_2_ALGEBRA],
        [COURSE_CREATE_DTO_3_STATISTICS],
    ];
    #endregion

    #region CourseUpdateDto
    public static CourseUpdateDto COURSE_UPDATE_DTO_1_CALCULUS => new()
    {
        Id = 1,
        Name = "Calculus",
        Credits = 10,
        TeacherId = TeacherTestData.TEACHER_MODEL_1_ASSISTANT.Id
    };

    public static CourseUpdateDto COURSE_UPDATE_DTO_2_ALGEBRA => new()
    {
        Id = 2,
        Name = "Linear Algebra",
        Credits = 6,
        TeacherId = TeacherTestData.TEACHER_MODEL_2_ASSOCIATED.Id
    };

    public static CourseUpdateDto COURSE_UPDATE_DTO_3_STATISTICS => new()
    {
        Id = 3,
        Name = "Probability and Statistics",
        Credits = 6,
        TeacherId = TeacherTestData.TEACHER_MODEL_1_ASSISTANT.Id
    };

    public static readonly IEnumerable<object[]> CourseUpdateDtoTestCases =
    [
        [COURSE_UPDATE_DTO_1_CALCULUS],
        [COURSE_UPDATE_DTO_2_ALGEBRA],
        [COURSE_UPDATE_DTO_3_STATISTICS],
    ];
    #endregion
}
