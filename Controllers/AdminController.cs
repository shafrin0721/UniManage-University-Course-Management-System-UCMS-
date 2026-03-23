using System.Linq;
using System.Web.Mvc;
using UniManage.DAL;
using UniManage.Filters;
using UniManage.Models;

namespace UniManage.Controllers
{
    [AuthorizeRole("Administrator")]
    public class AdminController : Controller
    {
        private readonly UcmsDbContext db = new UcmsDbContext();

        public ActionResult Dashboard()
        {
            var vm = new AdminDashboardViewModel
            {
                TotalUsers = db.AppUsers.Count(),
                TotalCourses = db.Courses.Count(),
                TotalEnrollments = db.Enrollments.Count(),
                TotalAssignments = db.Assignments.Count()
            };

            return View(vm);
        }
    }
}
