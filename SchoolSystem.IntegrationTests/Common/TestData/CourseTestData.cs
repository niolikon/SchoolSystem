using SchoolSystem.Core.Course;


namespace SchoolSystem.IntegrationTests.Common.TestData;

public record CourseTestData
{
    public static CourseModel COURSE_MODEL_1 => new()
    {
        Name = "Calculus",
        Credits = 10,
    };

    public static CourseModel COURSE_MODEL_2 => new()
    {
        Name = "Linear Algebra",
        Credits = 6,
    };

    public static CourseModel COURSE_MODEL_3 => new()
    {
        Name = "Probability and Statistics",
        Credits = 6,
    };
}
