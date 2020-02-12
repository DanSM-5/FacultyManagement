using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduardoS_300987998_A3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduardoS_300987998_A3.Controllers
{
    public class CRUDCourseController : Controller
    {
        ICourseRepository repository;
        public CRUDCourseController(ICourseRepository repo) => repository = repo;

        [Authorize]
        public ViewResult Update(int id) => View(
                                               repository.Courses
                                               .FirstOrDefault(c => c.CourseID == id));


        [HttpPost]
        [Authorize]
        public IActionResult Update(Course course)
        {
            if (ModelState.IsValid)
            {
                repository.Save(course);
                TempData["message"] = "Course edited successfully!!!";
                return RedirectToAction("Courses", "Course");
            }
            else
            {
                return View(course);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            Course course = repository.Delete(id);

            if (course != null)
            {
                TempData["message"] = $"{course.Name} was deleted successfully!!!";
            }

            return RedirectToAction("Courses", "Course");
        }
    }
}