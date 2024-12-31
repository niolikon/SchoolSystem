using SchoolSystem.Core.Student;

namespace SchoolSystem.Tests.Core.Student;

public static class StudentTestData
{
    #region StudentModel
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
    #endregion


    #region StudentDetails
    public static StudentDetailsDto STUDENT_DETAILS_DTO_1 => new()
    {
        Id = 1,
        FullName = "Sample Student 1",
        Email = "sample.student1@uni.ts"
    };

    public static StudentDetailsDto STUDENT_DETAILS_DTO_2 => new()
    {
        Id = 2,
        FullName = "Sample Student 2",
        Email = "sample.student2@uni.ts"
    };
    #endregion

    #region StudentCreateDto
    public static StudentCreateDto STUDENT_CREATE_DTO_1 => new()
    {
        FullName = "Sample Student 1",
        Email = "sample.student1@uni.ts"
    };

    public static StudentCreateDto STUDENT_CREATE_DTO_2 => new()
    {
        FullName = "Sample Student 2",
        Email = "sample.student2@uni.ts"
    };

    public static readonly IEnumerable<object[]> StudentCreateDtoTestCases =
    [
        [STUDENT_CREATE_DTO_1],
        [STUDENT_CREATE_DTO_2]
    ];
    #endregion


    #region StudentUpdateDto
    public static StudentUpdateDto STUDENT_UPDATE_DTO_1 => new()
    {
        Id = 1,
        FullName = "Sample Student 1",
        Email = "sample.student1@uni.ts"
    };

    public static StudentUpdateDto STUDENT_UPDATE_DTO_2 => new()
    {
        Id = 2,
        FullName = "Sample Student 2",
        Email = "sample.student2@uni.ts"
    };

    public static readonly IEnumerable<object[]> StudentUpdateDtoTestCases =
    [
        [STUDENT_UPDATE_DTO_1],
        [STUDENT_UPDATE_DTO_2]
    ];
    #endregion
}
