using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NedAcademy.Data;
using NedAcademy.Models;

namespace NedAcademy.Controllers
{
    [Authorize]
    public class NoticeController : Controller
    {
        private readonly AppDbContext _db;

        public NoticeController(AppDbContext db) => _db = db;

        public IActionResult Index()
        {
            var notices = _db.Notices.OrderByDescending(n => n.PublishedDate).ToList();
            return View(notices);
        }

        public IActionResult Details(int id)
        {
            var notice = _db.Notices.Find(id);
            if (notice == null) return NotFound();
            return View(notice);
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(NoticeModel notice)
        {
            if (!ModelState.IsValid) return View(notice);
            notice.CreatedAt = DateTime.Now;
            _db.Notices.Add(notice);
            _db.SaveChanges();
            TempData["Success"] = "Notice published successfully!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var notice = _db.Notices.Find(id);
            if (notice == null) return NotFound();
            return View(notice);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(NoticeModel notice)
        {
            if (!ModelState.IsValid) return View(notice);
            var existing = _db.Notices.Find(notice.Id);
            if (existing == null) return NotFound();

            existing.Title         = notice.Title;
            existing.Content       = notice.Content;
            existing.PublishedDate = notice.PublishedDate;
            existing.IsActive      = notice.IsActive;

            _db.Notices.Update(existing);
            _db.SaveChanges();
            TempData["Success"] = "Notice updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var notice = _db.Notices.Find(id);
            if (notice == null) return NotFound();
            return View(notice);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var notice = _db.Notices.Find(id);
            if (notice == null) return NotFound();
            _db.Notices.Remove(notice);
            _db.SaveChanges();
            TempData["Success"] = "Notice deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}
