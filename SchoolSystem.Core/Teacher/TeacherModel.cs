using SchoolSystem.Core.Common.BaseClasses;
using SchoolSystem.Core.Course;
using System.ComponentModel.DataAnnotations;


namespace SchoolSystem.Core.Teacher;

public enum AcademicPosition
{
    AdjunctProfessor,
    VisitingProfessor,
    PostdoctoralFellow,
    Lecturer,
    Instructor,

    AssistantProfessor,
    AssociateProfessor,
    FullProfessor,

    ResearchAssistant,
    ResearchAssociate,
    ResearchProfessor,
    SeniorResearcher,

    EmeritusProfessor,
    ChairProfessor,
    DistinguishedProfessor
}

public class TeacherModel : BaseModel<int>
{
    [Required, StringLength(maximumLength: 100, MinimumLength = 2)]
    public required string FullName { get; set; }

    [Required, StringLength(maximumLength: 20, MinimumLength = 2)]
    public required AcademicPosition Position { get; set; }

    [Required, DataType(DataType.EmailAddress), StringLength(maximumLength: 100, MinimumLength = 5)]
    public required string Email { get; set; }

    public virtual List<CourseModel> Courses { get; set; } = [];
}
