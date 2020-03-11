using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EduardoS_300987998_A3.Models;
using EduardoS_300987998_A3.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EduardoS_300987998_A3.Controllers
{
    public class FacultyController : Controller
    {
        private IFacultyRepository facultyRepo;
        private ICourseRepository courseRepo;

        public FacultyController(IFacultyRepository fRepo, ICourseRepository cRepo)
        {
          facultyRepo = fRepo;
          courseRepo = cRepo;
        }

        public ViewResult Faculty() => View(facultyRepo.Faculties);

        [Authorize(Roles = "Admin")]
        public ViewResult AddFaculty() => View(new Faculty());

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddToFaculty(Faculty faculty)
        {          
            if (ModelState.IsValid)
            {               
                await facultyRepo.Save(faculty);
                return RedirectToAction(nameof(Faculty));
            }
            else
            {
                ModelState.AddModelError("", "Error(s) found in your submission");
                return View(nameof(AddFaculty), faculty);
            }
        }

        [Authorize(Roles = "Admin, Manager")]
        public ViewResult AssignFaculty(int id = 0)
        {
            FacultyCourseViewModel model = new FacultyCourseViewModel
            {
                CourseRepository = courseRepo,
                CourseId = (TempData["number"] != null ? (int) TempData["number"] : 0)
            };
            //Creation of dropdown list
            //if id == 0 no option is selected by default
            model.List = id == 0 
                ? new SelectList(facultyRepo.Faculties, "FacultyID", "Name") 
                : new SelectList(facultyRepo.Faculties, "FacultyID", "Name", id.ToString()); 
            
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Manager")]
        public IActionResult AssignCourse(int cid)
        {
            TempData["number"] = cid;
            return RedirectToAction(nameof(AssignFaculty));
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> AssignToFaculty(int facultyID, List<int> ids)
        {
            if (ids.Count == 0)
            {
                TempData["invalid"] = "No course selected!";
                return RedirectToAction(nameof(AssignFaculty), new { id = facultyID });
            }
            if (facultyID == 0)
            {
                TempData["invalid"] = "Please select a Faculty";
                return RedirectToAction(nameof(AssignFaculty), new { id = facultyID });
            }

            Faculty faculty = await facultyRepo.Faculties.FirstOrDefaultAsync(f => f.FacultyID == facultyID);

            if (faculty != null)
            {
                var tasks = new List<Task<Tuple<Course, FacultyCourse>>>();
                foreach (int num in ids)
                {
                    tasks.Add(Assign(faculty, num));
                }

                var tuples = await Task.WhenAll(tasks);
                var validFC = tuples.Where(t => t != null).Select(t => t.Item2);
                var courses = tuples.Where(t => t != null).Select(t => t.Item1);

                var saveTask = Task.Run(async() => {
                    faculty.FacultyCourses.AddRange(validFC);
                    await facultyRepo.Save(faculty);
                });

                StringBuilder builder = new StringBuilder();
                var messageTask = Task.Run(() => {
                    builder.Append($"Course(s) assigned: ");
                    builder.Append(String.Join(", ", courses.Select(c => $"{c.Name}")));
                });

                await Task.WhenAll(saveTask, messageTask);
                TempData["message"] = builder.ToString();
                courses.Select(async(c) => await courseRepo.Save(c));
            }
            else
            {
                TempData["invalid"] = "Please select a Faculty";
            }
            return RedirectToAction(nameof(AssignFaculty), new { id = facultyID});
        }

        
        private async Task<Tuple<Course, FacultyCourse>> Assign(Faculty faculty, int courseID)
        {
            bool assignedCourse = faculty.FacultyCourses.Any(fc => fc.Course.CourseID == courseID);

            if (!assignedCourse)
            {
                Course course = await courseRepo.Courses.FirstOrDefaultAsync(c => c.CourseID == courseID);
                if (course != null)
                {
                    FacultyCourse facultyCourse = new FacultyCourse { Course = course, Faculty = faculty};
                    course.FacultyCourses.Add(facultyCourse);
                    Tuple<Course, FacultyCourse> content = new Tuple<Course, FacultyCourse>(course, facultyCourse);
                    return content; 
                }
            }
            return null;
        }

        [Authorize(Roles = "Admin, Manager")]
        public async Task<ViewResult> DataPage(int id) => View(await facultyRepo.Faculties
            .FirstOrDefaultAsync(f => f.FacultyID == id));

        [HttpPost]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> Unassign(int facultyId, int courseId)
        {
            if (facultyId != 0 && courseId != 0)
            {
                await facultyRepo.RemoveCourse(facultyId, courseId);
            }

            return RedirectToAction(nameof(DataPage), new { id = facultyId });
        }
    }
}