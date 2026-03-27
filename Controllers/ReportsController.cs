using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using UniManage.DAL;
using UniManage.Filters;
using UniManage.Models;

namespace UniManage.Controllers
{
    [AuthorizeRole("Lecturer", "Administrator")]
    public class ReportsController : Controller
    {
        private readonly UcmsDbContext db = new UcmsDbContext();

        public ActionResult Index()
        {
            int lecturerId = (int)Session["UserId"];

            // Grade Distribution — count of each grade range
            var allGrades = db.Submissions
                .Where(s => s.Grade.HasValue)
                .Select(s => s.Grade.Value)
                .ToList();

            var gradeDistribution = new Dictionary<string, int>
            {
                { "A (80-100)", allGrades.Count(g => g >= 80) },
                { "B (70-79)",  allGrades.Count(g => g >= 70 && g < 80) },
                { "C (60-69)",  allGrades.Count(g => g >= 60 && g < 70) },
                { "D (50-59)",  allGrades.Count(g => g >= 50 && g < 60) },
                { "F (0-49)",   allGrades.Count(g => g < 50) }
            };

            // Course Enrollment Stats
            var courseEnrollments = db.Courses
                .Where(c => c.LecturerId == lecturerId)
                .Select(c => new CourseEnrollmentStat
                {
                    CourseTitle = c.Title,
                    EnrolledCount = c.Enrollments.Count,
                    EnrollmentLimit = c.EnrollmentLimit
                }).ToList();

            // Student Performance — avg grade per student
            var studentPerformance = db.Submissions
                .Include(s => s.Student)
                .Where(s => s.Grade.HasValue)
                .GroupBy(s => s.Student)
                .Select(g => new StudentPerformanceStat
                {
                    StudentName = g.Key.FullName,
                    AverageGrade = g.Average(s => s.Grade.Value),
                    TotalSubmissions = g.Count()
                }).ToList();

            // Lecturer Workload
            var workload = new LecturerWorkloadStat
            {
                TotalCourses = courseEnrollments.Count,
                TotalAssignments = db.Assignments
                    .Count(a => a.Course.LecturerId == lecturerId),
                TotalSubmissions = db.Submissions
                    .Count(s => s.Assignment.Course.LecturerId == lecturerId),
                PendingGrading = db.Submissions
                    .Count(s => s.Assignment.Course.LecturerId == lecturerId
                        && !s.Grade.HasValue)
            };

            var vm = new ReportViewModel
            {
                GradeDistribution = gradeDistribution,
                CourseEnrollments = courseEnrollments,
                StudentPerformance = studentPerformance,
                Workload = workload
            };

            return View(vm);
        }
    }
}