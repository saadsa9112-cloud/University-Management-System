using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NedAcademy.Data;
using NedAcademy.Models;

namespace NedAcademy.Controllers
{
    [Authorize]
    public class CourseController : Controller
    {
        private readonly AppDbContext _db;

        public CourseController(AppDbContext db) => _db = db;

        public IActionResult Index()
        {
            var courses = _db.Courses
                .Include(c => c.Department)
                .Include(c => c.Students)
                .OrderBy(c => c.Name)
                .ToList();
            return View(courses);
        }

        public IActionResult Details(int id)
        {
            var course = _db.Courses
                .Include(c => c.Department)
                .Include(c => c.Students)
                .FirstOrDefault(c => c.Id == id);
            if (course == null) return NotFound();
            return View(course);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Departments = _db.Departments.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CourseModel course)
        {
            if (course.DepartmentId > 0 && !_db.Departments.Any(d => d.Id == course.DepartmentId))
                ModelState.AddModelError("DepartmentId", "Selected department does not exist.");

            if (!ModelState.IsValid)
            {
                ViewBag.Departments = _db.Departments.ToList();
                return View(course);
            }
            _db.Courses.Add(course);
            _db.SaveChanges();
            TempData["Success"] = $"Course \"{course.Name}\" was added successfully!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var course = _db.Courses.Find(id);
            if (course == null) return NotFound();
            ViewBag.Departments = _db.Departments.ToList();
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CourseModel course)
        {
            if (course.DepartmentId > 0 && !_db.Departments.Any(d => d.Id == course.DepartmentId))
                ModelState.AddModelError("DepartmentId", "Selected department does not exist.");

            if (!ModelState.IsValid)
            {
                ViewBag.Departments = _db.Departments.ToList();
                return View(course);
            }
            var existing = _db.Courses.Find(course.Id);
            if (existing == null) return NotFound();

            existing.Name         = course.Name;
            existing.Code         = course.Code;
            existing.Credits      = course.Credits;
            existing.Duration     = course.Duration;
            existing.Description  = course.Description;
            existing.DepartmentId = course.DepartmentId;

            _db.Courses.Update(existing);
            _db.SaveChanges();
            TempData["Success"] = $"Course \"{existing.Name}\" was updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var course = _db.Courses.Include(c => c.Department).FirstOrDefault(c => c.Id == id);
            if (course == null) return NotFound();
            return View(course);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteConfirmed(int id)
        {
            var course = _db.Courses.Include(c => c.Students).FirstOrDefault(c => c.Id == id);
            if (course == null) return NotFound();

            if (course.Students.Any())
            {
                TempData["Error"] = "Cannot delete this course — students are currently enrolled in it.";
                return RedirectToAction(nameof(Index));
            }

            _db.Courses.Remove(course);
            _db.SaveChanges();
            TempData["Success"] = $"Course \"{course.Name}\" was deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
}
