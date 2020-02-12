using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduardoS_300987998_A3.Models.ViewModels
{
    public class FacultyCourseViewModel
    {
        public ICourseRepository CourseRepository { get; set; }
        public int? CourseId { get; set; }
        public string DropDownListName { get; } = "facultyID";
        public string DropDownDefaultValue { get; } = "Faculty";
        public SelectList List { get; set; }
    }
}