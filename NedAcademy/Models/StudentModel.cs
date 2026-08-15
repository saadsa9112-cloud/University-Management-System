using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NedAcademy.Models
{
    public class StudentModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [StringLength(200)]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Invalid phone number")]
        [StringLength(20)]
        public string? Phone { get; set; }

        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; } = DateTime.Now.AddYears(-18);

        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; } = "Male";

        [StringLength(500)]
        public string? Address { get; set; }

        [StringLength(100)]
        [Display(Name = "Education Level")]
        public string? Education { get; set; }

        [Required(ErrorMessage = "Roll Number is required")]
        [StringLength(30)]
        [Display(Name = "Roll Number")]
        public string RollNumber { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "Fees must be a positive number")]
        [Display(Name = "Fees (PKR)")]
        public decimal Fees { get; set; }

        public string? ImageUrl { get; set; }

        [Display(Name = "Enrollment Date")]
        [DataType(DataType.Date)]
        public DateTime EnrollmentDate { get; set; } = DateTime.Now;

        // Foreign Keys — proper navigation, no redundant string fields
        [Required(ErrorMessage = "Department is required")]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
        public DepartmentModel? Department { get; set; }

        [Required(ErrorMessage = "Course is required")]
        [Display(Name = "Course")]
        public int CourseId { get; set; }
        public CourseModel? Course { get; set; }
    }
}
