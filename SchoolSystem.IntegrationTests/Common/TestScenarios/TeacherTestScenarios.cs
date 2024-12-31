using SchoolSystem.Core.Course;
using SchoolSystem.Core.Student;
using SchoolSystem.Core.Teacher;
using SchoolSystem.IntegrationTests.Common.TestData;

namespace SchoolSystem.IntegrationTests.Common.TestScenarios;

public static class TeacherTestScenarios
{
    public static object[] Empty => [];

    public static object[] SingleTeacher { 
        get 
        {
            CourseModel course = CourseTestData.COURSE_MODEL_1;
            StudentModel student = StudentTestData.STUDENT_MODEL_1;
            TeacherModel teacher = TeacherTestData.TEACHER_MODEL_1;

            teacher.Courses.Add(course);
            student.Courses.Add(course);
            course.Teacher = teacher;
            course.Students.Add(student);

            object[] result = [teacher, student, course];
            return result;
        } 
    }
}
