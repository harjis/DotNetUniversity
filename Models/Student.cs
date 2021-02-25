using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetUniversity.Models
{
    public class Student
    {
        public int Id { get; set; }
        
        [Required, StringLength(50, MinimumLength = 2)]
        public string LastName { get; set; }
        
        [Required, StringLength(50, MinimumLength = 2), Column("FirstName"), Display(Name = "First Name")]
        public string FirstMidName { get; set; }
        
        [
            DataType(DataType.Date),
            DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true),
            Display(Name = "Enrollment Date")
        ]
        public DateTime EnrollmentDate { get; set; }
        
        public ICollection<Enrollment> Enrollments { get; set; }

        public string FullName => $"{LastName} {FirstMidName}";
    }
}