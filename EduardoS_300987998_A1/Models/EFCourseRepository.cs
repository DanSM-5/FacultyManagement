using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduardoS_300987998_A3.Infraestructure;
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

        public async Task<Course> Delete(int id)
        {
            Course course = await Courses.FirstOrDefaultAsync(c => c.CourseID == id);

            if (course != null)
            {
                if (course.FacultyCourses.Count > 0)
                {
                    var removedFacultyCourses = new List<Task<FacultyCourse>>();
                    foreach (Faculty faculty in Faculties.Where(f => f.FacultyCourses.Any(fc => fc.Course.CourseID == id)))
                    {
                        removedFacultyCourses.Add(Task.Run(() => faculty.FacultyCourses
                                   .FirstOrDefault(fc => fc.Course.CourseID == id)));
                    }
                    var facultyCourses = await Task.WhenAll(removedFacultyCourses);
                    context.FacultyCourses.RemoveRange(facultyCourses);
                }

                context.Courses.Remove(course);
                context.SaveChanges();
            }

            return course;
        }

        public async Task Save(Course course)
        {
            if (course.CourseID != 0)
            {
                Course courseEntry = await context.Courses.FirstOrDefaultAsync(c => c.CourseID == course.CourseID);
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
            }
            else
            {
                context.Courses.Add(course);
            }

            await context.SaveChangesAsync();
        }
    }
}
