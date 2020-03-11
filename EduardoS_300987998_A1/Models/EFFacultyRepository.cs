using EduardoS_300987998_A3.Infraestructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduardoS_300987998_A3.Models
{
    public class EFFacultyRepository : IFacultyRepository
    {
        private ApplicationDbContext context;

        public EFFacultyRepository(ApplicationDbContext ctx) => context = ctx;

        public IQueryable<Faculty> Faculties => context.Faculties
            .Include(f => f.FacultyCourses)
                .ThenInclude(fc => fc.Course);

        public IQueryable<Course> Courses => context.Courses
            .Include(c => c.FacultyCourses)
                .ThenInclude(fc => fc.Faculty);

        public async Task Save(Faculty faculty)
        {
            context.AttachRange(faculty.FacultyCourses.Select(fc => fc.Course));

            if(faculty.FacultyID != 0)
            {
                Faculty facultyEntry = context.Faculties.FirstOrDefault(f => f.FacultyID == faculty.FacultyID);

                if (facultyEntry != null)
                {
                    facultyEntry.Name = faculty.Name;
                    facultyEntry.Email = faculty.Email;
                    facultyEntry.FacultyCourses = faculty.FacultyCourses;
                    facultyEntry.Notes = faculty.Notes;
                    facultyEntry.Phone = faculty.Phone;
                }
            }
            else
            {
                context.Faculties.Add(faculty);
            }

            await context.SaveChangesAsync();
        }

        public async Task<Faculty> Delete(int id)
        {
            Faculty faculty = await Faculties.FirstOrDefaultAsync(f => f.FacultyID == id);            

            if (faculty != null)
            {
                if (faculty.FacultyCourses.Count > 0)
                {
                    var removedFacultyCourses = new List<Task<FacultyCourse>>();
                    foreach (Course course in Courses.Where(c => c.FacultyCourses.Any(fc => fc.Faculty.FacultyID == id)))
                    {
                        removedFacultyCourses.Add(Task.Run(() => course.FacultyCourses
                                                    .FirstOrDefault(fc => fc.Faculty.FacultyID == id)));
                    }
                    var facultyCourses = await Task.WhenAll(removedFacultyCourses);
                    context.FacultyCourses.RemoveRange(facultyCourses);
                }

                context.Faculties.Remove(faculty);
                await context.SaveChangesAsync();
            }
            return faculty;
        }

        public async Task RemoveCourse(int facultyId, int courseId)
        {
            FacultyCourse facultyCourse = await context.FacultyCourses
                .FirstOrDefaultAsync(fc => fc.Course.CourseID == courseId && fc.Faculty.FacultyID == facultyId);
   
            context.FacultyCourses.Remove(facultyCourse);

            await context.SaveChangesAsync(); 
        }
    }
}
