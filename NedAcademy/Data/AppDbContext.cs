using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NedAcademy.Models;

namespace NedAcademy.Data;

public class AppDbContext : IdentityDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // DbSets
    public DbSet<DepartmentModel> Departments { get; set; }
    public DbSet<TeacherModel> Teachers { get; set; }
    public DbSet<StudentModel> Students { get; set; }
    public DbSet<CourseModel> Courses { get; set; }
    public DbSet<NoticeModel> Notices { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ── Relationships ─────────────────────────────────────────────────

        modelBuilder.Entity<StudentModel>()
            .HasOne(s => s.Department)
            .WithMany(d => d.Students)
            .HasForeignKey(s => s.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<StudentModel>()
            .HasOne(s => s.Course)
            .WithMany(c => c.Students)
            .HasForeignKey(s => s.CourseId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TeacherModel>()
            .HasOne(t => t.Department)
            .WithMany(d => d.Teachers)
            .HasForeignKey(t => t.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CourseModel>()
            .HasOne(c => c.Department)
            .WithMany(d => d.Courses)
            .HasForeignKey(c => c.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        // ── Seed Data ─────────────────────────────────────────────────────

        // Departments
        modelBuilder.Entity<DepartmentModel>().HasData(
            new DepartmentModel
            {
                Id = 1, Name = "Computer Science", Code = "CS",
                Description = "Department of Computer Science & Information Technology",
                HeadOfDept = "Dr. Ahmed Khan",
                CreatedAt = new DateTime(2020, 1, 1)
            },
            new DepartmentModel
            {
                Id = 2, Name = "Electrical Engineering", Code = "EE",
                Description = "Department of Electrical Engineering",
                HeadOfDept = "Dr. Sara Ali",
                CreatedAt = new DateTime(2020, 1, 1)
            },
            new DepartmentModel
            {
                Id = 3, Name = "Business Administration", Code = "BBA",
                Description = "Department of Business Administration & Management",
                HeadOfDept = "Prof. Zubair Sheikh",
                CreatedAt = new DateTime(2021, 3, 15)
            }
        );

        // Courses
        modelBuilder.Entity<CourseModel>().HasData(
            new CourseModel
            {
                Id = 1, Name = "Bachelor of Science in Computer Science", Code = "BSCS",
                Credits = 4, Duration = 48, DepartmentId = 1,
                Description = "4-year undergraduate degree in Computer Science"
            },
            new CourseModel
            {
                Id = 2, Name = "Diploma in Networking", Code = "DIN",
                Credits = 3, Duration = 12, DepartmentId = 1,
                Description = "Professional diploma in Computer Networking & Security"
            },
            new CourseModel
            {
                Id = 3, Name = "Bachelor of Electrical Engineering", Code = "BEE",
                Credits = 4, Duration = 48, DepartmentId = 2,
                Description = "4-year undergraduate degree in Electrical Engineering"
            },
            new CourseModel
            {
                Id = 4, Name = "Bachelor of Business Administration", Code = "BBA",
                Credits = 3, Duration = 36, DepartmentId = 3,
                Description = "3-year undergraduate degree in Business Administration"
            }
        );

        // Teachers
        modelBuilder.Entity<TeacherModel>().HasData(
            new TeacherModel
            {
                Id = 1, Name = "Sir Tasbeel Ahmad", Email = "tasbeel@nedacademy.edu",
                Phone = "0300-1234567", Qualification = "PhD Computer Science",
                Gender = "Male", DepartmentId = 1,
                JoiningDate = new DateTime(2020, 3, 15), Salary = 85000
            },
            new TeacherModel
            {
                Id = 2, Name = "Sir Mubashir Hussain", Email = "mubashir@nedacademy.edu",
                Phone = "0301-2345678", Qualification = "MS Networking & Security",
                Gender = "Male", DepartmentId = 1,
                JoiningDate = new DateTime(2019, 8, 1), Salary = 75000
            },
            new TeacherModel
            {
                Id = 3, Name = "Dr. Sara Ali", Email = "sara@nedacademy.edu",
                Phone = "0302-3456789", Qualification = "PhD Electrical Engineering",
                Gender = "Female", DepartmentId = 2,
                JoiningDate = new DateTime(2018, 1, 10), Salary = 95000
            },
            new TeacherModel
            {
                Id = 4, Name = "Prof. Zubair Sheikh", Email = "zubair@nedacademy.edu",
                Phone = "0303-4567890", Qualification = "MBA Finance",
                Gender = "Male", DepartmentId = 3,
                JoiningDate = new DateTime(2021, 3, 20), Salary = 80000
            }
        );

        // Students
        modelBuilder.Entity<StudentModel>().HasData(
            new StudentModel
            {
                Id = 1, Name = "Saad Ahmed", Email = "saad@student.nedacademy.edu",
                Phone = "0311-1234567", DateOfBirth = new DateTime(2004, 5, 15),
                Gender = "Male", Education = "Intermediate", RollNumber = "CS-2024-001",
                Fees = 50000, DepartmentId = 1, CourseId = 1,
                EnrollmentDate = new DateTime(2024, 9, 1)
            },
            new StudentModel
            {
                Id = 2, Name = "Umar Farooq", Email = "umar@student.nedacademy.edu",
                Phone = "0312-2345678", DateOfBirth = new DateTime(2003, 8, 20),
                Gender = "Male", Education = "Graduate", RollNumber = "CS-2024-002",
                Fees = 35000, DepartmentId = 1, CourseId = 2,
                EnrollmentDate = new DateTime(2024, 9, 1)
            },
            new StudentModel
            {
                Id = 3, Name = "Fatima Khan", Email = "fatima@student.nedacademy.edu",
                Phone = "0313-3456789", DateOfBirth = new DateTime(2004, 2, 10),
                Gender = "Female", Education = "Intermediate", RollNumber = "EE-2024-001",
                Fees = 55000, DepartmentId = 2, CourseId = 3,
                EnrollmentDate = new DateTime(2024, 9, 1)
            },
            new StudentModel
            {
                Id = 4, Name = "Ali Hassan", Email = "ali@student.nedacademy.edu",
                Phone = "0314-4567890", DateOfBirth = new DateTime(2005, 11, 25),
                Gender = "Male", Education = "Intermediate", RollNumber = "BBA-2024-001",
                Fees = 40000, DepartmentId = 3, CourseId = 4,
                EnrollmentDate = new DateTime(2024, 9, 1)
            }
        );

        // Notices
        modelBuilder.Entity<NoticeModel>().HasData(
            new NoticeModel
            {
                Id = 1, Title = "Welcome to NED Academy — Session 2024-25",
                Content = "We warmly welcome all new and returning students to NED Academy for the academic session 2024-2025. Please report to your respective departments by September 5th.",
                PublishedDate = new DateTime(2024, 9, 1), IsActive = true,
                CreatedAt = new DateTime(2024, 9, 1)
            },
            new NoticeModel
            {
                Id = 2, Title = "Mid-Term Examination Schedule",
                Content = "Mid-term examinations will be held from November 1 to November 15, 2024. Students are advised to check the exam timetable on the notice board of their respective departments.",
                PublishedDate = new DateTime(2024, 10, 15), IsActive = true,
                CreatedAt = new DateTime(2024, 10, 15)
            },
            new NoticeModel
            {
                Id = 3, Title = "Fee Submission Deadline",
                Content = "Students are reminded that the deadline for fee submission for the current semester is October 31, 2024. Late submissions will incur a fine.",
                PublishedDate = new DateTime(2024, 10, 10), IsActive = true,
                CreatedAt = new DateTime(2024, 10, 10)
            }
        );
    }
}
