using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetUniversity.Models
{
    public class Student
    {
        public int Id { get; set; }
        
        [StringLength(50)]
        public string LastName { get; set; }
        
        [StringLength(50), Column("FirstName")]
        public string FirstMidName { get; set; }
        
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "0:yyyy-MMM-dd", ApplyFormatInEditMode = true)]
        public DateTime EnrollmentDate { get; set; }
        
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}