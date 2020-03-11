using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduardoS_300987998_A3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduardoS_300987998_A3.Controllers
{
    public class CourseController : Controller
    {
        private ICourseRepository repository;

        public CourseController(ICourseRepository repo) => repository = repo;

        public ViewResult Courses() => View(repository.Courses);

        [Authorize]
        public ViewResult AddCourses() => View(new Course());

        [HttpPost]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> AddToCourses(Course course)
        {
            if (await repository.Courses.AnyAsync(c => c.Name == course.Name))
            {
                ModelState.AddModelError("", "Course name already exists");
            }

            if (ModelState.IsValid)
            {
                repository.Save(course);
                return RedirectToAction(nameof(Courses));
            }
            else
            {
                ModelState.AddModelError("", "Error(s) found in your submission");
                return View(nameof(AddCourses),course);
            } 
        }

        [Authorize]
        public async Task<ViewResult> DataPage(int id) => 
            View( await repository.Courses
                .FirstOrDefaultAsync(c => c.CourseID == id));
    }
}