using System.Linq;
using System.Web.Mvc;
using UniManage.DAL;
using UniManage.Filters;
using UniManage.Models;

namespace UniManage.Controllers
{
    [AuthorizeRole("Student")]
    public class StudentController : Controller
    {
        private readonly UcmsDbContext db = new UcmsDbContext();

        public ActionResult Dashboard()
        {
            int userId = (int)Session["UserId"];

            var vm = new StudentDashboardViewModel
            {
                Name = Session["FullName"].ToString(),
                Enrollments = db.Enrollments.Where(e => e.StudentId == userId).ToList(),
                Submissions = db.Submissions.Where(s => s.StudentId == userId).ToList(),
                UpcomingAssignments = db.Assignments.OrderBy(a => a.Deadline).Take(5).ToList()
            };

            return View(vm);
        }
    }
}
