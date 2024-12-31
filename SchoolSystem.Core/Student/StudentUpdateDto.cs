﻿using System.ComponentModel.DataAnnotations;

namespace SchoolSystem.Core.Student;

public class StudentUpdateDto
{
    [Required]
    public required int Id { get; set; }

    [Required, StringLength(maximumLength: 100, MinimumLength = 2)]
    public required string FullName { get; set; }

    [Required, DataType(DataType.EmailAddress), StringLength(maximumLength: 100, MinimumLength = 5)]
    public required string Email { get; set; }
}