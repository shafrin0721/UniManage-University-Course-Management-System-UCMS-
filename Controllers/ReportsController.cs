using System.Linq;
using System.Web.Mvc;
using UniManage.DAL;
using UniManage.Filters;
using UniManage.Models;

namespace UniManage.Controllers
{
    [AuthorizeRole("Administrator", "Lecturer")]
    public class ReportsController : Controller
    {
        private readonly UcmsDbContext db = new UcmsDbContext();

        public ActionResult Index()
        {
            var report = new ReportViewModel
            {
                Title = "Course Popularity Report",
                Labels = db.Courses.Select(c => c.CourseCode).ToList(),
                Values = db.Courses.Select(c => c.Enrollments.Count()).ToList()
            };

            ViewBag.AverageGrade = db.Submissions.Any(s => s.Grade != null)
                ? db.Submissions.Where(s => s.Grade != null).Average(s => s.Grade.Value).ToString("0.00")
                : "N/A";

            return View(report);
        }
    }
}