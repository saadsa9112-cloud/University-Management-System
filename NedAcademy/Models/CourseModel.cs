using System.ComponentModel.DataAnnotations;

namespace NedAcademy.Models
{
    public class CourseModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Course name is required")]
        [StringLength(150, ErrorMessage = "Name cannot exceed 150 characters")]
        [Display(Name = "Course Name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Course code is required")]
        [StringLength(15)]
        [Display(Name = "Course Code")]
        public string Code { get; set; } = string.Empty;

        [Range(1, 6, ErrorMessage = "Credits must be between 1 and 6")]
        [Display(Name = "Credit Hours")]
        public int Credits { get; set; } = 3;

        [StringLength(500)]
        public string? Description { get; set; }

        [Range(1, 120, ErrorMessage = "Duration must be between 1 and 120 months")]
        [Display(Name = "Duration (Months)")]
        public int Duration { get; set; } = 12;

        // Foreign Key
        [Required(ErrorMessage = "Department is required")]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
        public DepartmentModel? Department { get; set; }

        // Navigation
        public ICollection<StudentModel> Students { get; set; } = new List<StudentModel>();
    }
}
