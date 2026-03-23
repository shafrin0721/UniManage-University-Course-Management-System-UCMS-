using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using UniManage.DAL;
using UniManage.Filters;
using UniManage.Models;

namespace UniManage.Controllers
{
    public class CoursesController : Controller
    {
        private readonly UcmsDbContext db = new UcmsDbContext();

        [AuthorizeRole("Administrator", "Lecturer", "Student")]
        public ActionResult Index()
        {
            var data = db.Courses.Include(c => c.Lecturer).ToList();
            return View(data);
        }

        [AuthorizeRole("Administrator")]
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.LecturerId = new SelectList(db.AppUsers.Where(u => u.Role == "Lecturer"), "Id", "FullName");
            return View();
        }

        [AuthorizeRole("Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Course model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.LecturerId = new SelectList(db.AppUsers.Where(u => u.Role == "Lecturer"), "Id", "FullName", model.LecturerId);
                return View(model);
            }
            db.Courses.Add(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [AuthorizeRole("Administrator")]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var course = db.Courses.Find(id);
            ViewBag.LecturerId = new SelectList(db.AppUsers.Where(u => u.Role == "Lecturer"), "Id", "FullName", course.LecturerId);
            return View(course);
        }

        [AuthorizeRole("Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Course model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.LecturerId = new SelectList(db.AppUsers.Where(u => u.Role == "Lecturer"), "Id", "FullName", model.LecturerId);
                return View(model);
            }
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [AuthorizeRole("Administrator")]
        [HttpGet]
        public ActionResult Delete(int id)
        {
            return View(db.Courses.Find(id));
        }

        [AuthorizeRole("Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var course = db.Courses.Find(id);
            db.Courses.Remove(course);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
