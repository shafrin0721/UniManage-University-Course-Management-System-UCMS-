using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using UniManage.DAL;
using UniManage.Filters;
using UniManage.Models;

namespace UniManage.Controllers
{
    public class AssignmentsController : Controller
    {
        private readonly UcmsDbContext db = new UcmsDbContext();

        [AuthorizeRole("Lecturer", "Student")]
        public ActionResult Index()
        {
            return View(db.Assignments.Include(a => a.Course).OrderBy(a => a.Deadline).ToList());
        }

        [AuthorizeRole("Lecturer")]
        [HttpGet]
        public ActionResult Create()
        {
            int lecturerId = (int)Session["UserId"];
            ViewBag.CourseId = new SelectList(db.Courses.Where(c => c.LecturerId == lecturerId), "Id", "Title");
            return View();
        }

        [AuthorizeRole("Lecturer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Assignment model)
        {
            if (!ModelState.IsValid)
            {
                int lecturerId = (int)Session["UserId"];
                ViewBag.CourseId = new SelectList(db.Courses.Where(c => c.LecturerId == lecturerId), "Id", "Title", model.CourseId);
                return View(model);
            }
            db.Assignments.Add(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [AuthorizeRole("Student")]
        [HttpGet]
        public ActionResult Submit(int id)
        {
            ViewBag.Assignment = db.Assignments.Include(a => a.Course).FirstOrDefault(a => a.Id == id);
            return View(new Submission { AssignmentId = id });
        }

        [AuthorizeRole("Student")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Submit(Submission model)
        {
            model.StudentId = (int)Session["UserId"];
            model.SubmittedAt = DateTime.Now;

            if (!ModelState.IsValid)
            {
                ViewBag.Assignment = db.Assignments.Include(a => a.Course).FirstOrDefault(a => a.Id == model.AssignmentId);
                return View(model);
            }

            db.Submissions.Add(model);
            db.SaveChanges();
            return RedirectToAction("Dashboard", "Student");
        }

        [AuthorizeRole("Lecturer")]
        [HttpGet]
        public ActionResult Grade(int id)
        {
            var submission = db.Submissions.Include(s => s.Student).Include(s => s.Assignment).FirstOrDefault(s => s.Id == id);
            return View(submission);
        }

        [AuthorizeRole("Lecturer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Grade(int id, decimal? grade, string feedback)
        {
            var submission = db.Submissions.Find(id);
            submission.Grade = grade;
            submission.Feedback = feedback;
            db.SaveChanges();
            return RedirectToAction("Dashboard", "Lecturer");
        }
    }
}
