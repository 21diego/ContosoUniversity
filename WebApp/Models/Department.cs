using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class Department
    {
        [Key]
        [Required]
        public int DepartmentID { get; set; }
        [StringLength(20)]
        [Required]
        public string Title { get; set; }
        [StringLength(50)]
        [Required]
        public string Description { get; set; }
    }
}