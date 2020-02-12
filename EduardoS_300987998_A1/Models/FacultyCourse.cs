using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduardoS_300987998_A3.Models
{
    public class FacultyCourse
    {
        public int FacultyCourseID { get; set; }
        public Faculty Faculty { get; set; }
        public Course Course { get; set; }

    }
}
