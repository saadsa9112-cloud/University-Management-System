using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NedAcademy.Data;
using NedAcademy.Models;

namespace NedAcademy.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        private readonly AppDbContext _db;

        public StudentController(AppDbContext db) => _db = db;

        // GET: /Student
        public IActionResult Index()
        {
            var students = _db.Students
                .Include(s => s.Department)
                .Include(s => s.Course)
                .OrderByDescending(s => s.EnrollmentDate)
                .ToList();
            return View(students);
        }

        // GET: /Student/Details/5
        public IActionResult Details(int id)
        {
            var student = _db.Students
                .Include(s => s.Department)
                .Include(s => s.Course)
                .FirstOrDefault(s => s.Id == id);

            if (student == null) return NotFound();
            return View(student);
        }

        // GET: /Student/Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Departments = _db.Departments.ToList();
            ViewBag.Courses     = _db.Courses.ToList();
            return View();
        }

        // POST: /Student/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(StudentModel student, IFormFile? imageFile)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Departments = _db.Departments.ToList();
                ViewBag.Courses     = _db.Courses.ToList();
                return View(student);
            }

            student.ImageUrl = SaveImage(imageFile, null, "students");

            _db.Students.Add(student);
            _db.SaveChanges();
            TempData["Success"] = $"Student \"{student.Name}\" was added successfully!";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Student/Edit/5
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var student = _db.Students.Find(id);
            if (student == null) return NotFound();

            ViewBag.Departments = _db.Departments.ToList();
            ViewBag.Courses     = _db.Courses.ToList();
            return View(student);
        }

        // POST: /Student/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(StudentModel student, IFormFile? imageFile)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Departments = _db.Departments.ToList();
                ViewBag.Courses     = _db.Courses.ToList();
                return View(student);
            }

            var existing = _db.Students.Find(student.Id);
            if (existing == null) return NotFound();

            // Update scalar fields
            existing.Name           = student.Name;
            existing.Email          = student.Email;
            existing.Phone          = student.Phone;
            existing.DateOfBirth    = student.DateOfBirth;
            existing.Gender         = student.Gender;
            existing.Address        = student.Address;
            existing.Education      = student.Education;
            existing.RollNumber     = student.RollNumber;
            existing.Fees           = student.Fees;
            existing.DepartmentId   = student.DepartmentId;
            existing.CourseId       = student.CourseId;
            existing.EnrollmentDate = student.EnrollmentDate;

            // Handle image update
            if (imageFile != null && imageFile.Length > 0)
                existing.ImageUrl = SaveImage(imageFile, existing.ImageUrl, "students");

            _db.Students.Update(existing);
            _db.SaveChanges();
            TempData["Success"] = $"Student \"{existing.Name}\" was updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Student/Delete/5
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var student = _db.Students
                .Include(s => s.Department)
                .Include(s => s.Course)
                .FirstOrDefault(s => s.Id == id);
            if (student == null) return NotFound();
            return View(student);
        }

        // POST: /Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var student = _db.Students.Find(id);
            if (student == null) return NotFound();

            DeleteImage(student.ImageUrl);
            _db.Students.Remove(student);
            _db.SaveChanges();
            TempData["Success"] = $"Student \"{student.Name}\" was deleted.";
            return RedirectToAction(nameof(Index));
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        private string? SaveImage(IFormFile? file, string? oldImageUrl, string folder)
        {
            if (file == null || file.Length == 0) return oldImageUrl;

            var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", folder);
            Directory.CreateDirectory(uploadFolder);

            // Delete old image safely
            DeleteImage(oldImageUrl);

            var uniqueName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName).ToLower()}";
            var filePath   = Path.Combine(uploadFolder, uniqueName);

            using var stream = new FileStream(filePath, FileMode.Create);
            file.CopyTo(stream);

            return $"uploads/{folder}/{uniqueName}";
        }

        private void DeleteImage(string? imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl)) return;
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imageUrl);
            if (System.IO.File.Exists(fullPath))
                System.IO.File.Delete(fullPath);
        }
    }
}
