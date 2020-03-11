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
    [Authorize(Roles = "Admin")]
    public class CRUDController : Controller
    {
        private IFacultyRepository repository;

        public CRUDController(IFacultyRepository repo) => repository = repo;

        public async Task<ViewResult> Update(int id) => View(
                                               await repository.Faculties
                                                    .FirstOrDefaultAsync(f => f.FacultyID == id)); 

        [HttpPost]
        public async Task<IActionResult> Update(Faculty faculty)
        {
            if (ModelState.IsValid)
            {
                await repository.Save(faculty);
                TempData["message"] = "Faculty edited successfully!!!";
                return RedirectToAction("Faculty","Faculty");
            }
            else
            {
                return View(faculty);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            Faculty faculty = await repository.Delete(id);

            if (faculty != null)
            {
                TempData["message"] = $"{faculty.Name} was deleted successfully";
            }

            return RedirectToAction("Faculty", "Faculty");
        }
    }
}