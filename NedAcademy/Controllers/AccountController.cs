using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NedAcademy.Models.ViewModels;

namespace NedAcademy.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<AccountController> logger)
        {
            _signInManager = signInManager;
            _userManager   = userManager;
            _roleManager   = roleManager;
            _logger        = logger;
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
                return View(model);

            // Try sign in
            var result = await _signInManager.PasswordSignInAsync(
                model.Email, model.Password, model.RememberMe, lockoutOnFailure: true);

            if (result.Succeeded)
            {
                // Validate that the user actually has the selected role
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null && !await _userManager.IsInRoleAsync(user, model.Role))
                {
                    // Wrong role selected — sign them out
                    await _signInManager.SignOutAsync();
                    ModelState.AddModelError(string.Empty,
                        $"This account does not have the '{model.Role}' role. Please select the correct role.");
                    return View(model);
                }

                _logger.LogInformation("User {Email} logged in as {Role}.", model.Email, model.Role);

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);

                return RedirectToAction("Index", "Home");
            }

            if (result.IsLockedOut)
            {
                _logger.LogWarning("User {Email} is locked out.", model.Email);
                ModelState.AddModelError(string.Empty, "Account is locked. Please try again in 10 minutes.");
                return View(model);
            }

            ModelState.AddModelError(string.Empty, "Invalid email or password. Please try again.");
            return View(model);
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            // SECURITY: Public registration always creates Faculty accounts only.
            // Admin accounts can only be created via UserManagement by an existing Admin.
            model.Role = "Faculty";
            ModelState.Remove("Role"); // clear any role validation errors

            if (!ModelState.IsValid)
                return View(model);

            // Check email not already taken
            if (await _userManager.FindByEmailAsync(model.Email) != null)
            {
                ModelState.AddModelError("Email", "An account with this email already exists.");
                return View(model);
            }

            var user = new IdentityUser
            {
                UserName       = model.Email,
                Email          = model.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                // Ensure Faculty role exists
                if (!await _roleManager.RoleExistsAsync("Faculty"))
                    await _roleManager.CreateAsync(new IdentityRole("Faculty"));

                await _userManager.AddToRoleAsync(user, "Faculty");

                _logger.LogInformation("New Faculty account created: {Email}.", model.Email);
                await _signInManager.SignInAsync(user, isPersistent: false);
                TempData["Success"] = "Welcome! Your Faculty account has been created.";
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(model);
        }


        // GET: /Account/AccessDenied
        [HttpGet]
        public IActionResult AccessDenied()
        {
            TempData["Error"] = "Access Denied: You do not have permission to access that section. Administrator privileges required.";
            return RedirectToAction("Index", "Home");
        }

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToAction("Login", "Account");
        }
    }
}
