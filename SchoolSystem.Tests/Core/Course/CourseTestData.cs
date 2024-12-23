using SchoolSystem.Core.Course;
using SchoolSystem.Tests.Core.Teacher;


namespace SchoolSystem.Tests.Core.Course;

public record CourseTestData
{
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

    public static CourseDto COURSE_DTO_1_CALCULUS => new()
    {
        Id = 1,
        Name = "Calculus",
        Credits = 10,
        TeacherId = TeacherTestData.TEACHER_DTO_1_ASSISTANT.Id
    };

    public static CourseDto COURSE_DTO_2_ALGEBRA => new()
    {
        Id = 2,
        Name = "Linear Algebra",
        Credits = 6,
        TeacherId = TeacherTestData.TEACHER_DTO_2_ASSOCIATED.Id
    };

    public static CourseDto COURSE_DTO_3_STATISTICS => new()
    {
        Id = 3,
        Name = "Probability and Statistics",
        Credits = 6,
        TeacherId = TeacherTestData.TEACHER_DTO_1_ASSISTANT.Id
    };

    public static readonly IEnumerable<object[]> CourseDtoTestCases =
    [
        [COURSE_DTO_1_CALCULUS],
        [COURSE_DTO_2_ALGEBRA],
        [COURSE_DTO_3_STATISTICS],
    ];

    public static readonly IEnumerable<object[]> CourseModelAndRelatedDtoTestCases =
    [
        [COURSE_MODEL_1_CALCULUS, COURSE_DTO_1_CALCULUS],
        [COURSE_MODEL_2_ALGEBRA, COURSE_DTO_2_ALGEBRA],
        [COURSE_MODEL_3_STATISTICS, COURSE_DTO_3_STATISTICS],
    ];
}
