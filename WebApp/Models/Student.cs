using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class Student
    {
        [Key]
        [Required]
        public int StudentID { get; set; }
        [Required]
        [StringLength(30)]
        public string FullName { get; set; }
        [Required]
        [StringLength(50)]
        public string Email { get; set; }

    }
}