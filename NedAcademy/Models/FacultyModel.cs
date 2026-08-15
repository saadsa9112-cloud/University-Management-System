// FacultyModel has been replaced by TeacherModel.
// This file is kept as a placeholder to avoid breaking any lingering references.
// See Models/TeacherModel.cs for the current implementation.
namespace NedAcademy.Models
{
    [Obsolete("Use TeacherModel instead. This class is kept for compatibility only.")]
    public class FacultyModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Email { get; set; }
    }
}
