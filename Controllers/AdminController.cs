using System.Linq;
using System.Web.Mvc;
using UniManage.DAL;
using UniManage.Filters;
using UniManage.Models;


namespace UniManage.Controllers
{
    public class AdminController : Controller
    {
        private readonly UcmsDbContext db = new UcmsDbContext();

        public ActionResult Dashboard()
        {
            ViewData["Title"] = "Administrator Dashboard";

            ViewBag.TotalUsers = db.AppUsers.Count();
            ViewBag.TotalLecturers = db.AppUsers.Count(x => x.Role == "Lecturer");
            ViewBag.TotalStudents = db.AppUsers.Count(x => x.Role == "Student");
            ViewBag.TotalCourses = db.Courses.Count();
            ViewBag.TotalAssignments = db.Assignments.Count();
            ViewBag.TotalEnrollments = db.Enrollments.Count();

            var recentCourses = db.Courses
                .OrderByDescending(c => c.Id)
                .Take(5)
                .ToList();

            return View(recentCourses);
        }
    }
}