using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using UniManage.DAL;
using UniManage.Filters;
using UniManage.Models;
using System.Net;


namespace UniManage.Controllers
{
    public class CoursesController : Controller
    {
        private readonly UcmsDbContext db = new UcmsDbContext();

        public ActionResult Index()
        {
            ViewData["Title"] = "Course Management";

            var courses = db.Courses
                .OrderBy(c => c.CourseCode)
                .ToList();

            return View(courses);
        }

        public ActionResult Create()
        {
            ViewData["Title"] = "Create Course";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Course course)
        {
            if (ModelState.IsValid)
            {
                db.Courses.Add(course);
                db.SaveChanges();
                TempData["Success"] = "Course created successfully.";
                return RedirectToAction("Index");
            }

            return View(course);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var course = db.Courses.Find(id);
            if (course == null)
                return HttpNotFound();

            ViewData["Title"] = "Edit Course";
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Course course)
        {
            if (ModelState.IsValid)
            {
                var existing = db.Courses.Find(course.Id);
                if (existing == null)
                    return HttpNotFound();

                existing.CourseCode = course.CourseCode;
                existing.Title = course.Title;
                existing.Description = course.Description;
                existing.EnrollmentLimit = course.EnrollmentLimit;
                existing.LecturerId = course.LecturerId;

                db.SaveChanges();
                TempData["Success"] = "Course updated successfully.";
                return RedirectToAction("Index");
            }

            return View(course);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var course = db.Courses.Find(id);
            if (course == null)
                return HttpNotFound();

            ViewData["Title"] = "Delete Course";
            return View(course);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var course = db.Courses.Find(id);
            if (course == null)
                return HttpNotFound();

            db.Courses.Remove(course);
            db.SaveChanges();

            TempData["Success"] = "Course deleted successfully.";
            return RedirectToAction("Index");
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var course = db.Courses.Find(id);
            if (course == null)
                return HttpNotFound();

            ViewData["Title"] = "Course Details";
            return View(course);
        }
    }
}