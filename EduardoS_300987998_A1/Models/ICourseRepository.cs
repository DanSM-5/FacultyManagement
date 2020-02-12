using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduardoS_300987998_A3.Models
{
    public interface ICourseRepository
    {
        IQueryable<Course> Courses{ get; }
        void Save(Course course);
        Course Delete(int id);
    }
}
