using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DotNetUniversity.Models
{
    public class Student
    {
        public int Id { get; set; }
        
        [StringLength(50)]
        public string LastName { get; set; }
        
        [StringLength(50)]
        public string FirstMidName { get; set; }
        
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "0:yyyy-MMM-dd", ApplyFormatInEditMode = true)]
        public DateTime EnrollmentDate { get; set; }
        
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}