using System.Collections.Generic;

namespace UniManage.Models
{
    public class ReportViewModel
    {
        public Dictionary<string, int> GradeDistribution { get; set; }
        public List<CourseEnrollmentStat> CourseEnrollments { get; set; }
        public List<StudentPerformanceStat> StudentPerformance { get; set; }
        public LecturerWorkloadStat Workload { get; set; }
    }

    public class CourseEnrollmentStat
    {
        public string CourseTitle { get; set; }
        public int EnrolledCount { get; set; }
        public int EnrollmentLimit { get; set; }
    }

    public class StudentPerformanceStat
    {
        public string StudentName { get; set; }
        public decimal AverageGrade { get; set; }
        public int TotalSubmissions { get; set; }
    }

    public class LecturerWorkloadStat
    {
        public int TotalCourses { get; set; }
        public int TotalAssignments { get; set; }
        public int TotalSubmissions { get; set; }
        public int PendingGrading { get; set; }
    }
}