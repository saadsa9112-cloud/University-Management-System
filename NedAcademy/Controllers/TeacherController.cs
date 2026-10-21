using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NedAcademy.Data;
using NedAcademy.Models;

namespace NedAcademy.Controllers
{
    [Authorize]
    public class TeacherController : Controller
    {
        private readonly AppDbContext _db;

        public TeacherController(AppDbContext db) => _db = db;

        public IActionResult Index()
        {
            var teachers = _db.Teachers
                .Include(t => t.Department)
                .OrderBy(t => t.Name)
                .ToList();
            return View(teachers);
        }

        public IActionResult Details(int id)
        {
            var teacher = _db.Teachers
                .Include(t => t.Department)
                .FirstOrDefault(t => t.Id == id);
            if (teacher == null) return NotFound();
            return View(teacher);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Departments = _db.Departments.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TeacherModel teacher, IFormFile? imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var ext = Path.GetExtension(imageFile.FileName).ToLower();
                var allowedExts = new[] { ".jpg", ".jpeg", ".png", ".webp", ".gif" };
                if (!allowedExts.Contains(ext))
                    ModelState.AddModelError(string.Empty, "Invalid photo format. Only JPG, PNG, WEBP, or GIF are allowed.");
                else if (imageFile.Length > 5 * 1024 * 1024)
                    ModelState.AddModelError(string.Empty, "Photo size cannot exceed 5MB.");
            }

            if (teacher.DepartmentId > 0 && !_db.Departments.Any(d => d.Id == teacher.DepartmentId))
                ModelState.AddModelError("DepartmentId", "Selected department does not exist.");

            if (!ModelState.IsValid)
            {
                ViewBag.Departments = _db.Departments.ToList();
                return View(teacher);
            }

            teacher.ImageUrl = SaveImage(imageFile, null);
            _db.Teachers.Add(teacher);
            _db.SaveChanges();
            TempData["Success"] = $"Teacher \"{teacher.Name}\" was added successfully!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var teacher = _db.Teachers.Find(id);
            if (teacher == null) return NotFound();
            ViewBag.Departments = _db.Departments.ToList();
            return View(teacher);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TeacherModel teacher, IFormFile? imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var ext = Path.GetExtension(imageFile.FileName).ToLower();
                var allowedExts = new[] { ".jpg", ".jpeg", ".png", ".webp", ".gif" };
                if (!allowedExts.Contains(ext))
                    ModelState.AddModelError(string.Empty, "Invalid photo format. Only JPG, PNG, WEBP, or GIF are allowed.");
                else if (imageFile.Length > 5 * 1024 * 1024)
                    ModelState.AddModelError(string.Empty, "Photo size cannot exceed 5MB.");
            }

            if (teacher.DepartmentId > 0 && !_db.Departments.Any(d => d.Id == teacher.DepartmentId))
                ModelState.AddModelError("DepartmentId", "Selected department does not exist.");

            if (!ModelState.IsValid)
            {
                ViewBag.Departments = _db.Departments.ToList();
                return View(teacher);
            }

            var existing = _db.Teachers.Find(teacher.Id);
            if (existing == null) return NotFound();

            existing.Name         = teacher.Name;
            existing.Email        = teacher.Email;
            existing.Phone        = teacher.Phone;
            existing.Qualification= teacher.Qualification;
            existing.Gender       = teacher.Gender;
            existing.Address      = teacher.Address;
            existing.JoiningDate  = teacher.JoiningDate;
            existing.Salary       = teacher.Salary;
            existing.DepartmentId = teacher.DepartmentId;

            if (imageFile != null && imageFile.Length > 0)
                existing.ImageUrl = SaveImage(imageFile, existing.ImageUrl);

            _db.Teachers.Update(existing);
            _db.SaveChanges();
            TempData["Success"] = $"Teacher \"{existing.Name}\" was updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var teacher = _db.Teachers.Include(t => t.Department).FirstOrDefault(t => t.Id == id);
            if (teacher == null) return NotFound();
            return View(teacher);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteConfirmed(int id)
        {
            var teacher = _db.Teachers.Find(id);
            if (teacher == null) return NotFound();
            DeleteImage(teacher.ImageUrl);
            _db.Teachers.Remove(teacher);
            _db.SaveChanges();
            TempData["Success"] = $"Teacher \"{teacher.Name}\" was deleted.";
            return RedirectToAction(nameof(Index));
        }

        private string? SaveImage(IFormFile? file, string? oldUrl)
        {
            if (file == null || file.Length == 0) return oldUrl;
            var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "teachers");
            Directory.CreateDirectory(folder);
            DeleteImage(oldUrl);
            var name = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName).ToLower()}";
            using var stream = new FileStream(Path.Combine(folder, name), FileMode.Create);
            file.CopyTo(stream);
            return $"uploads/teachers/{name}";
        }

        private void DeleteImage(string? url)
        {
            if (string.IsNullOrEmpty(url)) return;
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", url);
            if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
        }
    }
}
