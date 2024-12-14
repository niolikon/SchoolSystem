using SchoolSystem.Core.Course;


namespace SchoolSystem.IntegrationTests.Infrastructure.Course;

public static class CourseTestData
{
    private static readonly CourseModel _COURSE_MODEL_1 = new()
    {
        Name = "Calculus",
        Credits = 10,
    };

    private static readonly CourseModel _COURSE_MODEL_2 = new()
    {
        Name = "Linear Algebra",
        Credits = 6,
    };

    private static readonly CourseModel _COURSE_MODEL_3 = new()
    {
        Name = "Probability and Statistics",
        Credits = 6,
    };

    public static CourseModel COURSE_MODEL_1 => _COURSE_MODEL_1.Clone;
    public static CourseModel COURSE_MODEL_2 => _COURSE_MODEL_2.Clone;
    public static CourseModel COURSE_MODEL_3 => _COURSE_MODEL_3.Clone;
}
