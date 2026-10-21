namespace NedAcademy.Models.ViewModels
{
    public class DashboardViewModel
    {
        // Summary Counts
        public int TotalStudents { get; set; }
        public int TotalTeachers { get; set; }
        public int TotalDepartments { get; set; }
        public int TotalCourses { get; set; }
        public decimal TotalRevenue { get; set; }
        public int ActiveNoticesCount { get; set; }
        public int MaleStudentsCount { get; set; }
        public int FemaleStudentsCount { get; set; }

        // Recent Data
        public List<StudentModel> RecentStudents { get; set; } = new();
        public List<NoticeModel> RecentNotices { get; set; } = new();

        // Chart Data
        public List<string> DepartmentNames { get; set; } = new();
        public List<int> StudentsByDepartment { get; set; } = new();
        public List<int> TeachersByDepartment { get; set; } = new();
        public List<decimal> RevenueByDepartment { get; set; } = new();
    }
}
