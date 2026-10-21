using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NedAcademy.Models.ViewModels;

namespace NedAcademy.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserManagementController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserManagementController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: /UserManagement/Details?id=xxx  (called via AJAX for modal)
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var roles    = await _userManager.GetRolesAsync(user);
            var lockout  = await _userManager.IsLockedOutAsync(user);

            return Json(new
            {
                id              = user.Id,
                email           = user.Email,
                emailConfirmed  = user.EmailConfirmed,
                roles           = roles,
                isLockedOut     = lockout,
                lockoutEnd      = user.LockoutEnd?.ToString("dd MMM yyyy HH:mm") ?? "—",
                accessFailedCount = user.AccessFailedCount
            });
        }

        // POST: /UserManagement/ResetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(string id, string newPassword, string confirmPassword)
        {
            if (string.IsNullOrWhiteSpace(newPassword) || newPassword != confirmPassword)
                return Json(new { success = false, message = "Passwords do not match or are empty." });

            if (newPassword.Length < 6)
                return Json(new { success = false, message = "Password must be at least 6 characters." });

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return Json(new { success = false, message = "User not found." });

            var token  = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

            if (result.Succeeded)
            {
                // Also unlock the account if locked
                await _userManager.ResetAccessFailedCountAsync(user);
                await _userManager.SetLockoutEndDateAsync(user, null);
                return Json(new { success = true, message = $"Password for '{user.Email}' has been reset successfully." });
            }

            var errors = string.Join(" ", result.Errors.Select(e => e.Description));
            return Json(new { success = false, message = errors });
        }

        // GET: /UserManagement
        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.ToList();
            var model = new List<UserListViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                model.Add(new UserListViewModel
                {
                    Id    = user.Id,
                    Email = user.Email ?? string.Empty,
                    Roles = roles.ToList()
                });
            }

            return View(model);
        }

        // GET: /UserManagement/CreateUser (or /UserManagement/CreateStaff)
        [HttpGet]
        [Route("UserManagement/CreateUser")]
        [Route("UserManagement/CreateStaff")]
        public IActionResult CreateStaff()
        {
            return View("CreateStaff");
        }

        // POST: /UserManagement/CreateUser (or /UserManagement/CreateStaff)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("UserManagement/CreateUser")]
        [Route("UserManagement/CreateStaff")]
        public async Task<IActionResult> CreateStaff(CreateUserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (await _userManager.FindByEmailAsync(model.Email) != null)
            {
                ModelState.AddModelError("Email", "A user with this email already exists.");
                return View(model);
            }

            var user = new IdentityUser
            {
                UserName       = model.Email,
                Email          = model.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var err in result.Errors)
                    ModelState.AddModelError(string.Empty, err.Description);
                return View(model);
            }

            if (!await _roleManager.RoleExistsAsync(model.Role))
                await _roleManager.CreateAsync(new IdentityRole(model.Role));

            await _userManager.AddToRoleAsync(user, model.Role);

            TempData["Success"] = $"User '{model.Email}' created and assigned to '{model.Role}' role.";
            return RedirectToAction(nameof(Index));
        }

        // POST: /UserManagement/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser?.Id == id)
            {
                TempData["Error"] = "You cannot delete your own account.";
                return RedirectToAction(nameof(Index));
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            await _userManager.DeleteAsync(user);
            TempData["Success"] = $"User '{user.Email}' was deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
}
