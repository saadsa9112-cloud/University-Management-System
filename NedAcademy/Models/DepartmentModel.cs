using System.ComponentModel.DataAnnotations;

namespace NedAcademy.Models
{
    public class DepartmentModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Department name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        [Display(Name = "Department Name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Department code is required")]
        [StringLength(10, ErrorMessage = "Code cannot exceed 10 characters")]
        public string Code { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        [StringLength(100)]
        [Display(Name = "Head of Department")]
        public string? HeadOfDept { get; set; }

        [Display(Name = "Established")]
        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation Properties
        public ICollection<TeacherModel> Teachers { get; set; } = new List<TeacherModel>();
        public ICollection<StudentModel> Students { get; set; } = new List<StudentModel>();
        public ICollection<CourseModel> Courses { get; set; } = new List<CourseModel>();
    }
}
