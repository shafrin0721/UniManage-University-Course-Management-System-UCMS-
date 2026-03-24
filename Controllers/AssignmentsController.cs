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
            TempData["Success"] = "Assignment created successfully.";
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
            var submission = db.Submissions
                .Include(s => s.Student)
                .Include(s => s.Assignment)
                .FirstOrDefault(s => s.Id == id);
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
            TempData["Success"] = "Submission graded successfully.";
            return RedirectToAction("Dashboard", "Lecturer");
        }



        [AuthorizeRole("Lecturer")]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var assignment = db.Assignments.Find(id);
            if (assignment == null) return HttpNotFound();

            int lecturerId = (int)Session["UserId"];
            ViewBag.CourseId = new SelectList(
                db.Courses.Where(c => c.LecturerId == lecturerId),
                "Id", "Title", assignment.CourseId);

            return View(assignment);
        }

        [AuthorizeRole("Lecturer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Assignment model)
        {
            if (!ModelState.IsValid)
            {
                int lecturerId = (int)Session["UserId"];
                ViewBag.CourseId = new SelectList(
                    db.Courses.Where(c => c.LecturerId == lecturerId),
                    "Id", "Title", model.CourseId);
                return View(model);
            }

            var existing = db.Assignments.Find(model.Id);
            existing.Title = model.Title;
            existing.Instructions = model.Instructions;
            existing.Deadline = model.Deadline;
            existing.CourseId = model.CourseId;
            db.SaveChanges();

            TempData["Success"] = "Assignment updated successfully.";
            return RedirectToAction("Index");
        }

        [AuthorizeRole("Lecturer")]
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var assignment = db.Assignments
                .Include(a => a.Course)
                .FirstOrDefault(a => a.Id == id);
            if (assignment == null) return HttpNotFound();
            return View(assignment);
        }

        [AuthorizeRole("Lecturer")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var assignment = db.Assignments.Find(id);
            db.Assignments.Remove(assignment);
            db.SaveChanges();

            TempData["Success"] = "Assignment deleted successfully.";
            return RedirectToAction("Index");
        }

        [AuthorizeRole("Lecturer")]
        public ActionResult Submissions(int id)
        {
            var assignment = db.Assignments
                .Include(a => a.Course)
                .FirstOrDefault(a => a.Id == id);

            if (assignment == null) return HttpNotFound();

            var submissions = db.Submissions
                .Include(s => s.Student)
                .Where(s => s.AssignmentId == id)
                .ToList();

            ViewBag.Assignment = assignment;
            return View(submissions);
        }
    }
}