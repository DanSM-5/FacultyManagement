using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EduardoS_300987998_A3.Models
{
    public class EFCourseRepository : ICourseRepository
    {
        private ApplicationDbContext context;

        public EFCourseRepository(ApplicationDbContext ctx) => context = ctx;

        public IQueryable<Course> Courses => context.Courses
            .Include(c => c.FacultyCourses)
                .ThenInclude(fc => fc.Faculty);

        public IQueryable<Faculty> Faculties => context.Faculties
            .Include(f => f.FacultyCourses)
                .ThenInclude(fc => fc.Course);

        public Course Delete(int id)
        {
            Course course = Courses.FirstOrDefault(c => c.CourseID == id);

            if (course != null)
            {
                if (course.FacultyCourses.Count > 0)
                {
                    foreach (Faculty faculty in Faculties.Where(f => f.FacultyCourses.Any(fc => fc.Course.CourseID == course.CourseID)))
                    {
                        FacultyCourse facultyCourse = course.FacultyCourses
                            .FirstOrDefault(fc => fc.Faculty.FacultyID == faculty.FacultyID);

                        faculty.FacultyCourses.Remove(facultyCourse);
                        course.FacultyCourses.Remove(facultyCourse);

                        context.FacultyCourses.Remove(facultyCourse);
                    } 
                }

                context.Courses.Remove(course);
                context.SaveChanges();
            }

            return course;
        }

        public void Save(Course course)
        {
            Course courseEntry = context.Courses.FirstOrDefault(c => c.CourseID == course.CourseID);

            if (courseEntry != null)
            {
                if (courseEntry != null)
                {
                    courseEntry.Name = course.Name;
                    courseEntry.Description = course.Description;
                    courseEntry.ShortName = course.ShortName;
                    courseEntry.FacultyCourses = course.FacultyCourses;
                }
            }

            else
            {
                context.Courses.Add(course);
            }

            context.SaveChanges();
        }
    }
}
