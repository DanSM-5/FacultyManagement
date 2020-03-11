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
    public class CRUDCourseController : Controller
    {
        ICourseRepository repository;
        public CRUDCourseController(ICourseRepository repo) => repository = repo;

        [Authorize]
        public async Task<ViewResult> Update(int id) => View(
                                               await repository.Courses
                                                    .FirstOrDefaultAsync(c => c.CourseID == id));


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
        public async Task<IActionResult> Delete(int id)
        {
            Course course = await repository.Delete(id);

            if (course != null)
            {
                TempData["message"] = $"{course.Name} was deleted successfully!!!";
            }

            return RedirectToAction("Courses", "Course");
        }
    }
}