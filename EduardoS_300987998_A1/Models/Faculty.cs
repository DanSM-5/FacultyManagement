using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EduardoS_300987998_A3.Models
{
    public class Faculty
    {
        public int FacultyID { get; set; }

        [Required(ErrorMessage = "Please enter a Name")]
        public string Name { get; set; }
        [BindNever]
        public List<FacultyCourse> FacultyCourses { get; set; } = new List<FacultyCourse>();
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$", ErrorMessage = "Please enter an Email")]
        [Required]
        public string Email { get; set; }
        [RegularExpression(@"^\d\d\d\d\d\d\d\d\d\d$", ErrorMessage = "Please enter a valid number")]
        [Required]
        public string Phone { get; set; }
        public string Notes { get; set; }        
    }
}
