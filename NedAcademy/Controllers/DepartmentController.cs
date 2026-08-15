using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NedAcademy.Data;
using NedAcademy.Models;

namespace NedAcademy.Controllers
{
    [Authorize] // Any logged-in user can view/create/edit
    public class DepartmentController : Controller
    {
        private readonly AppDbContext _db;

        public DepartmentController(AppDbContext db) => _db = db;

        public IActionResult Index()
        {
            var departments = _db.Departments
                .Include(d => d.Teachers)
                .Include(d => d.Students)
                .Include(d => d.Courses)
                .OrderBy(d => d.Name)
                .ToList();
            return View(departments);
        }

        public IActionResult Details(int id)
        {
            var dept = _db.Departments
                .Include(d => d.Teachers)
                .Include(d => d.Students)
                .Include(d => d.Courses)
                .FirstOrDefault(d => d.Id == id);
            if (dept == null) return NotFound();
            return View(dept);
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DepartmentModel dept)
        {
            if (!ModelState.IsValid) return View(dept);
            dept.CreatedAt = DateTime.Now;
            _db.Departments.Add(dept);
            _db.SaveChanges();
            TempData["Success"] = $"Department \"{dept.Name}\" was created successfully!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var dept = _db.Departments.Find(id);
            if (dept == null) return NotFound();
            return View(dept);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(DepartmentModel dept)
        {
            if (!ModelState.IsValid) return View(dept);
            var existing = _db.Departments.Find(dept.Id);
            if (existing == null) return NotFound();

            existing.Name       = dept.Name;
            existing.Code       = dept.Code;
            existing.Description= dept.Description;
            existing.HeadOfDept = dept.HeadOfDept;

            _db.Departments.Update(existing);
            _db.SaveChanges();
            TempData["Success"] = $"Department \"{existing.Name}\" was updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var dept = _db.Departments
                .Include(d => d.Teachers)
                .Include(d => d.Students)
                .FirstOrDefault(d => d.Id == id);
            if (dept == null) return NotFound();
            return View(dept);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteConfirmed(int id)
        {
            var dept = _db.Departments
                .Include(d => d.Teachers)
                .Include(d => d.Students)
                .Include(d => d.Courses)
                .FirstOrDefault(d => d.Id == id);
            if (dept == null) return NotFound();

            if (dept.Students.Any() || dept.Teachers.Any() || dept.Courses.Any())
            {
                TempData["Error"] = "Cannot delete this department — it still has students, teachers, or courses assigned to it.";
                return RedirectToAction(nameof(Index));
            }

            _db.Departments.Remove(dept);
            _db.SaveChanges();
            TempData["Success"] = $"Department \"{dept.Name}\" was deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
}
