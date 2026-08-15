using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NedAcademy.Data;
using NedAcademy.Models;
using NedAcademy.Models.ViewModels;
using System.Diagnostics;

namespace NedAcademy.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _db;

        public HomeController(ILogger<HomeController> logger, AppDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            var departments = _db.Departments.ToList();

            var vm = new DashboardViewModel
            {
                TotalStudents    = _db.Students.Count(),
                TotalTeachers    = _db.Teachers.Count(),
                TotalDepartments = _db.Departments.Count(),
                TotalCourses     = _db.Courses.Count(),

                RecentStudents = _db.Students
                    .Include(s => s.Course)
                    .Include(s => s.Department)
                    .OrderByDescending(s => s.EnrollmentDate)
                    .Take(5)
                    .ToList(),

                RecentNotices = _db.Notices
                    .Where(n => n.IsActive)
                    .OrderByDescending(n => n.PublishedDate)
                    .Take(4)
                    .ToList(),

                DepartmentNames       = departments.Select(d => d.Name).ToList(),
                StudentsByDepartment  = departments.Select(d => _db.Students.Count(s => s.DepartmentId == d.Id)).ToList(),
                TeachersByDepartment  = departments.Select(d => _db.Teachers.Count(t => t.DepartmentId == d.Id)).ToList(),
            };

            return View(vm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}