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

        public void Save(Faculty faculty)
        {
            context.AttachRange(faculty.FacultyCourses.Select(fc => fc.Course));

            if(context.Faculties.Any(f => f.FacultyID == faculty.FacultyID))
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

            context.SaveChanges();
        }

        public Faculty Delete(int id)
        {
            Faculty faculty = Faculties.FirstOrDefault(f => f.FacultyID == id);            

            if (faculty != null)
            {
                if (faculty.FacultyCourses.Count > 0)
                {
                    foreach (Course course in Courses.Where(c => c.FacultyCourses.Any(fc => fc.Faculty.FacultyID == faculty.FacultyID)))
                    {
                        RemoveCourse(faculty, course, saveChanges: false);
                    }
                }

                context.Faculties.Remove(faculty);
                context.SaveChanges();
            }
            return faculty;
        }

        public bool RemoveCourse(Faculty faculty, Course course, bool saveChanges = true)
        {
            FacultyCourse facultyCourse = course.FacultyCourses
                            .FirstOrDefault(fc => fc.Faculty.FacultyID == faculty.FacultyID);

            bool removedFaculty = faculty.FacultyCourses.Remove(facultyCourse);
            bool removedCourse = course.FacultyCourses.Remove(facultyCourse);

            if (removedCourse && removedFaculty)
            {           
                context.FacultyCourses.Remove(facultyCourse);

                if (saveChanges)
                {
                    context.SaveChanges(); 
                }
            }

            return removedCourse && removedFaculty;
        }
    }
}
