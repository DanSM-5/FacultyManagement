using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EduardoS_300987998_A3.Models
{
    public class Course
    {
        public int CourseID { get; set; }

        [Required(ErrorMessage = "Please write the course name")]
        public string Name { get; set; }

        [RegularExpression(@"^[A-Z][a-z]{3}-\d{3}$", ErrorMessage ="Please enter a valid short name!!!")]
        [Required]
        public string ShortName { get; set; }
        public string Description { get; set; }

        [BindNever]
        public ICollection<FacultyCourse> FacultyCourses { get; set; }
    }
}
