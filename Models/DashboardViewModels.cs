using System.Collections.Generic;

namespace UniManage.Models
{
    public class StudentDashboardViewModel
    {
        public string Name { get; set; }
        public List<Enrollment> Enrollments { get; set; }
        public List<Submission> Submissions { get; set; }
        public List<Assignment> UpcomingAssignments { get; set; }
    }

    public class LecturerDashboardViewModel
    {
        public string Name { get; set; }
        public List<Course> Courses { get; set; }
        public List<Submission> RecentSubmissions { get; set; }
    }

    public class AdminDashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalCourses { get; set; }
        public int TotalEnrollments { get; set; }
        public int TotalAssignments { get; set; }
    }

    public class ReportViewModel
    {
        public string Title { get; set; }
        public List<string> Labels { get; set; }
        public List<int> Values { get; set; }
    }
}
