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
                TotalStudents        = _db.Students.Count(),
                TotalTeachers        = _db.Teachers.Count(),
                TotalDepartments     = departments.Count,
                TotalCourses         = _db.Courses.Count(),
                TotalRevenue         = _db.Students.Sum(s => (decimal?)s.Fees) ?? 0,
                ActiveNoticesCount   = _db.Notices.Count(n => n.IsActive),
                MaleStudentsCount    = _db.Students.Count(s => s.Gender == "Male"),
                FemaleStudentsCount  = _db.Students.Count(s => s.Gender == "Female"),

                RecentStudents = _db.Students
                    .Include(s => s.Course)
                    .Include(s => s.Department)
                    .OrderByDescending(s => s.EnrollmentDate)
                    .Take(6)
                    .ToList(),

                RecentNotices = _db.Notices
                    .Where(n => n.IsActive)
                    .OrderByDescending(n => n.PublishedDate)
                    .Take(5)
                    .ToList(),

                DepartmentNames       = departments.Select(d => d.Name).ToList(),
                StudentsByDepartment  = departments.Select(d => _db.Students.Count(s => s.DepartmentId == d.Id)).ToList(),
                TeachersByDepartment  = departments.Select(d => _db.Teachers.Count(t => t.DepartmentId == d.Id)).ToList(),
                RevenueByDepartment   = departments.Select(d => _db.Students.Where(s => s.DepartmentId == d.Id).Sum(s => (decimal?)s.Fees) ?? 0).ToList()
            };

            return View(vm);
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? statusCode = null)
        {
            var code = statusCode ?? HttpContext.Response.StatusCode;
            string msg = code switch
            {
                404 => "The page or record you requested could not be found.",
                403 => "You do not have permission to access this resource.",
                500 => "An unexpected server error occurred.",
                _ => "An unexpected issue occurred while processing your request."
            };

            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                StatusCode = code,
                Message = msg
            });
        }
    }
}