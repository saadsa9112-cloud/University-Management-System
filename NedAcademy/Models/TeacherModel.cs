using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NedAcademy.Models
{
    public class TeacherModel
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

        [StringLength(200)]
        public string? Qualification { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; } = "Male";

        [StringLength(500)]
        public string? Address { get; set; }

        [Display(Name = "Joining Date")]
        [DataType(DataType.Date)]
        public DateTime JoiningDate { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "Salary must be a positive number")]
        [Display(Name = "Salary (PKR)")]
        public decimal Salary { get; set; }

        public string? ImageUrl { get; set; }

        // Foreign Key
        [Required(ErrorMessage = "Department is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid Department")]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
        public DepartmentModel? Department { get; set; }
    }
}
