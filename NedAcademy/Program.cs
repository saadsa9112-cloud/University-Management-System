using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NedAcademy.Data;

var builder = WebApplication.CreateBuilder(args);

// ── Services ──────────────────────────────────────────────────────────────────

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromHours(8);
    options.SlidingExpiration = true;
});

// ── Build ─────────────────────────────────────────────────────────────────────

var app = builder.Build();

// ── Middleware ────────────────────────────────────────────────────────────────

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Root path redirects to Login (or UserManagement/Home if logged in)
app.MapGet("/", context =>
{
    context.Response.Redirect("/Account/Login");
    return Task.CompletedTask;
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// ── Seed Admin User ───────────────────────────────────────────────────────────

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var db = services.GetRequiredService<AppDbContext>();
        await SeedAdminAsync(userManager, roleManager);
        await SeedDemoDataAsync(db);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the admin user or demo data.");
    }
}

app.Run();

// ── Seed Method ───────────────────────────────────────────────────────────────

static async Task SeedAdminAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
{
    // Create roles
    string[] roles = { "Admin", "Faculty" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }

    // Create default admin user
    const string adminEmail = "admin@nedacademy.edu";
    const string adminPassword = "Admin@123";

    if (await userManager.FindByEmailAsync(adminEmail) == null)
    {
        var admin = new IdentityUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(admin, adminPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(admin, "Admin");
        }
    }
}

static async Task SeedDemoDataAsync(AppDbContext db)
{
    // Seed Departments if empty
    if (!await db.Departments.AnyAsync())
    {
        var csDept = new NedAcademy.Models.DepartmentModel
        {
            Name = "Computer Science & IT",
            Code = "CSIT",
            Description = "Department of Computer Science and Information Technology",
            HeadOfDept = "Prof. Dr. Muhammad Ali",
            CreatedAt = DateTime.Now.AddMonths(-18)
        };
        var seDept = new NedAcademy.Models.DepartmentModel
        {
            Name = "Software Engineering",
            Code = "SE",
            Description = "Department of Software Engineering and System Architecture",
            HeadOfDept = "Dr. Asma Khan",
            CreatedAt = DateTime.Now.AddMonths(-16)
        };
        var eeDept = new NedAcademy.Models.DepartmentModel
        {
            Name = "Electrical Engineering",
            Code = "EE",
            Description = "Department of Electrical and Telecommunication Engineering",
            HeadOfDept = "Engr. Tariq Mehmood",
            CreatedAt = DateTime.Now.AddMonths(-24)
        };
        var dsDept = new NedAcademy.Models.DepartmentModel
        {
            Name = "Data Science & AI",
            Code = "DSAI",
            Description = "Advanced analytics, machine learning, and artificial intelligence programs",
            HeadOfDept = "Dr. Zeeshan Haider",
            CreatedAt = DateTime.Now.AddMonths(-10)
        };

        db.Departments.AddRange(csDept, seDept, eeDept, dsDept);
        await db.SaveChangesAsync();

        // Seed Courses
        var csCourse = new NedAcademy.Models.CourseModel
        {
            Name = "BS Computer Science",
            Code = "BS-CS",
            Credits = 4,
            Duration = 48,
            Description = "Comprehensive 4-year degree in core computing, algorithms, and systems",
            DepartmentId = csDept.Id
        };
        var seCourse = new NedAcademy.Models.CourseModel
        {
            Name = "BS Software Engineering",
            Code = "BS-SE",
            Credits = 4,
            Duration = 48,
            Description = "Modern full-stack engineering, cloud architecture, and agile practices",
            DepartmentId = seDept.Id
        };
        var eeCourse = new NedAcademy.Models.CourseModel
        {
            Name = "BE Electrical Engineering",
            Code = "BE-EE",
            Credits = 4,
            Duration = 48,
            Description = "Power electronics, circuit design, and signal processing",
            DepartmentId = eeDept.Id
        };
        var dsCourse = new NedAcademy.Models.CourseModel
        {
            Name = "MS Data Science",
            Code = "MS-DS",
            Credits = 3,
            Duration = 24,
            Description = "Advanced machine learning, statistical modeling, and big data systems",
            DepartmentId = dsDept.Id
        };

        db.Courses.AddRange(csCourse, seCourse, eeCourse, dsCourse);
        await db.SaveChangesAsync();

        // Seed Teachers
        db.Teachers.AddRange(
            new NedAcademy.Models.TeacherModel
            {
                Name = "Prof. Dr. Muhammad Ali",
                Email = "m.ali@nedacademy.edu",
                Phone = "0300-1122334",
                Qualification = "PhD Computer Science (Stanford)",
                Gender = "Male",
                Salary = 260000,
                JoiningDate = DateTime.Now.AddYears(-5),
                DepartmentId = csDept.Id
            },
            new NedAcademy.Models.TeacherModel
            {
                Name = "Dr. Asma Khan",
                Email = "asma.khan@nedacademy.edu",
                Phone = "0321-4455667",
                Qualification = "PhD Software Engineering",
                Gender = "Female",
                Salary = 230000,
                JoiningDate = DateTime.Now.AddYears(-3),
                DepartmentId = seDept.Id
            },
            new NedAcademy.Models.TeacherModel
            {
                Name = "Engr. Tariq Mehmood",
                Email = "tariq.m@nedacademy.edu",
                Phone = "0333-8899001",
                Qualification = "MS Electrical Engineering",
                Gender = "Male",
                Salary = 210000,
                JoiningDate = DateTime.Now.AddYears(-4),
                DepartmentId = eeDept.Id
            },
            new NedAcademy.Models.TeacherModel
            {
                Name = "Dr. Sadia Rehman",
                Email = "sadia.r@nedacademy.edu",
                Phone = "0312-3344556",
                Qualification = "PhD Artificial Intelligence",
                Gender = "Female",
                Salary = 225000,
                JoiningDate = DateTime.Now.AddYears(-2),
                DepartmentId = dsDept.Id
            }
        );
        await db.SaveChangesAsync();

        // Seed Students
        db.Students.AddRange(
            new NedAcademy.Models.StudentModel
            {
                Name = "Hamza Farooq",
                Email = "hamza.farooq@student.ned.edu",
                RollNumber = "CS-2024-001",
                Phone = "0301-2345678",
                Gender = "Male",
                DateOfBirth = DateTime.Now.AddYears(-20),
                Education = "HSC Pre-Engineering",
                Fees = 75000,
                DepartmentId = csDept.Id,
                CourseId = csCourse.Id,
                EnrollmentDate = DateTime.Now.AddMonths(-6)
            },
            new NedAcademy.Models.StudentModel
            {
                Name = "Ayesha Siddiqui",
                Email = "ayesha.s@student.ned.edu",
                RollNumber = "SE-2024-012",
                Phone = "0313-9876543",
                Gender = "Female",
                DateOfBirth = DateTime.Now.AddYears(-19),
                Education = "A-Levels (Cambridge)",
                Fees = 85000,
                DepartmentId = seDept.Id,
                CourseId = seCourse.Id,
                EnrollmentDate = DateTime.Now.AddMonths(-5)
            },
            new NedAcademy.Models.StudentModel
            {
                Name = "Bilal Ahmed",
                Email = "bilal.ahmed@student.ned.edu",
                RollNumber = "EE-2024-045",
                Phone = "0345-5678901",
                Gender = "Male",
                DateOfBirth = DateTime.Now.AddYears(-21),
                Education = "HSC Pre-Engineering",
                Fees = 80000,
                DepartmentId = eeDept.Id,
                CourseId = eeCourse.Id,
                EnrollmentDate = DateTime.Now.AddMonths(-7)
            },
            new NedAcademy.Models.StudentModel
            {
                Name = "Fatima Zahra",
                Email = "fatima.z@student.ned.edu",
                RollNumber = "DS-2024-003",
                Phone = "0322-1122445",
                Gender = "Female",
                DateOfBirth = DateTime.Now.AddYears(-22),
                Education = "BS Computer Science",
                Fees = 95000,
                DepartmentId = dsDept.Id,
                CourseId = dsCourse.Id,
                EnrollmentDate = DateTime.Now.AddMonths(-3)
            },
            new NedAcademy.Models.StudentModel
            {
                Name = "Zubair Khan",
                Email = "zubair.k@student.ned.edu",
                RollNumber = "CS-2024-019",
                Phone = "0300-8877665",
                Gender = "Male",
                DateOfBirth = DateTime.Now.AddYears(-20),
                Education = "HSC Pre-Engineering",
                Fees = 75000,
                DepartmentId = csDept.Id,
                CourseId = csCourse.Id,
                EnrollmentDate = DateTime.Now.AddMonths(-4)
            },
            new NedAcademy.Models.StudentModel
            {
                Name = "Mariam Tariq",
                Email = "mariam.t@student.ned.edu",
                RollNumber = "SE-2024-034",
                Phone = "0331-4433221",
                Gender = "Female",
                DateOfBirth = DateTime.Now.AddYears(-20),
                Education = "HSC Pre-Engineering",
                Fees = 85000,
                DepartmentId = seDept.Id,
                CourseId = seCourse.Id,
                EnrollmentDate = DateTime.Now.AddMonths(-2)
            }
        );
        await db.SaveChangesAsync();

        // Seed Notices
        db.Notices.AddRange(
            new NedAcademy.Models.NoticeModel
            {
                Title = "Fall 2026 Semester Admissions Officially Opened",
                Content = "Admissions for undergraduate and graduate degree programs are officially open. Eligible candidates can submit applications online via the admission portal.",
                PublishedDate = DateTime.Now.AddDays(-2),
                IsActive = true
            },
            new NedAcademy.Models.NoticeModel
            {
                Title = "Mid-Term Examinations Guidelines & Fee Clearance",
                Content = "Midterm examinations will commence next month. All enrolled students are requested to clear outstanding dues and collect their examination slips.",
                PublishedDate = DateTime.Now.AddDays(-6),
                IsActive = true
            },
            new NedAcademy.Models.NoticeModel
            {
                Title = "Annual Student Technology & Project Exhibition 2026",
                Content = "NED Academy is organizing its signature annual innovation expo. Final year project teams can submit booth registrations before the deadline.",
                PublishedDate = DateTime.Now.AddDays(-12),
                IsActive = true
            },
            new NedAcademy.Models.NoticeModel
            {
                Title = "Campus High-Speed Wi-Fi & Digital Lab Upgrades",
                Content = "The campus IT network has been upgraded with fiber connectivity and dedicated compute clusters for AI & Data Science research students.",
                PublishedDate = DateTime.Now.AddDays(-18),
                IsActive = true
            }
        );
        await db.SaveChangesAsync();
    }
}
