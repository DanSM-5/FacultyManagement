using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduardoS_300987998_A3.Models;
using EduardoS_300987998_A3.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        public IActionResult AddToFaculty(Faculty faculty)
        {          
            if (ModelState.IsValid)
            {               
                facultyRepo.Save(faculty);
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
        public IActionResult AssignToFaculty(int facultyID, List<int> ids)
        {
            Faculty faculty = facultyRepo.Faculties.FirstOrDefault(f => f.FacultyID == facultyID);

            if (ids.Count == 0)
            {
                TempData["invalid"] = "No course selected!";
            }

            if (faculty != null)
            {
                foreach (int num in ids)
                {
                    Assign(faculty, num);
                }
            }
            else
            {
                TempData["invalid"] = "Please select a Faculty";
            }


            return RedirectToAction(nameof(AssignFaculty), new { id = facultyID});
        }

        
        private void Assign(Faculty faculty, int courseID)
        {
            Course course = courseRepo.Courses.FirstOrDefault(c => c.CourseID == courseID);
            bool assignedCourse = faculty.FacultyCourses.Any(fc => fc.Course.CourseID == courseID);

            if (course != null && !assignedCourse)
            {
                FacultyCourse facultyCourse = new FacultyCourse { Course = course, Faculty = faculty };

                faculty.FacultyCourses.Add(facultyCourse);
                facultyRepo.Save(faculty);

                course.FacultyCourses.Add(facultyCourse);
                courseRepo.Save(course);

                if (TempData["message"] == null)
                {
                    TempData["message"] = "Course(s) assigned! - " + course.Name;
                }
                else
                {
                    TempData["message"] += ", " + course.Name;
                }
            }
            else if(assignedCourse)
            {
                if (TempData["repeated"] == null)
                {
                    TempData["repeated"] = "Faculty already teaches: " + course.Name;
                }
                else
                {
                    TempData["repeated"] += ", " + course.Name;
                }
            }
        }

        [Authorize(Roles = "Admin, Manager")]
        public ViewResult DataPage(int id) => View(facultyRepo.Faculties
            .FirstOrDefault(f => f.FacultyID == id));

        [HttpPost]
        [Authorize(Roles = "Admin, Manager")]
        public IActionResult Unassign(int facultyId, int courseId)
        {
            Faculty faculty = facultyRepo.Faculties.FirstOrDefault(f => f.FacultyID == facultyId);
            Course course = courseRepo.Courses.FirstOrDefault(c => c.CourseID == courseId);

            if (faculty != null && course != null)
            {
                facultyRepo.RemoveCourse(faculty, course);
            }

            return RedirectToAction(nameof(DataPage), new { id = facultyId });
        }
    }
}