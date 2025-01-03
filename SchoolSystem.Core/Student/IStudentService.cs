﻿using SchoolSystem.Core.Common.BaseInterfaces;

namespace SchoolSystem.Core.Student;

public interface IStudentService : IBaseService<StudentDetailsDto, StudentCreateDto, StudentUpdateDto>
{
}