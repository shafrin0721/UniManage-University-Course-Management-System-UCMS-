using System.Linq;
using System.Web.Mvc;
using UniManage.DAL;
using UniManage.Filters;
using UniManage.Models;
using System.Data.Entity;

namespace UniManage.Controllers
{
    [AuthorizeRole("Lecturer")]
    public class LecturerController : Controller
    {
        private readonly UcmsDbContext db = new UcmsDbContext();

        public ActionResult Dashboard()
        {
            int lecturerId = (int)Session["UserId"];

            var vm = new LecturerDashboardViewModel
            {
                Name = Session["FullName"].ToString(),
                Courses = db.Courses.Where(c => c.LecturerId == lecturerId).ToList(),
                RecentSubmissions = db.Submissions
                    .Include(s => s.Student)
                    .Include(s => s.Assignment)
                    .OrderByDescending(s => s.SubmittedAt)
                    .Take(10)
                    .ToList()
            };

            return View(vm);
        }
    }
}
