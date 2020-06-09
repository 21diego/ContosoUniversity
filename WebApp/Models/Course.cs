using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class Course
    {
        [Key]
        [Required]
        public int CourseID { get; set; }
        [StringLength(20)]
        [Required]
        public string Title { get; set; }
        [Required]
        public int Capacity { get; set; }
        [Required]
        public int DepartmentID { get; set; }
        [Required]
        public int InstructorID { get; set; }

        public virtual Department Department { get; set; }
        public virtual Instructor Instructor { get; set; }
    }
}