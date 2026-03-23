using System;
using System.Linq;
using System.Web.Mvc;
using UniManage.DAL;
using UniManage.Filters;
using UniManage.Models;

namespace UniManage.Controllers
{
    [AuthorizeRole("Student")]
    public class EnrollmentController : Controller
    {
        private readonly UcmsDbContext db = new UcmsDbContext();

        public ActionResult Browse()
        {
            return View(db.Courses.ToList());
        }

        public ActionResult MyCourses()
        {
            int userId = (int)Session["UserId"];
            var enrollments = db.Enrollments.Where(e => e.StudentId == userId).ToList();
            return View(enrollments);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Enroll(int courseId)
        {
            int userId = (int)Session["UserId"];
            var course = db.Courses.Find(courseId);
            if (course == null) return RedirectToAction("Browse");

            bool already = db.Enrollments.Any(e => e.StudentId == userId && e.CourseId == courseId);
            int currentCount = db.Enrollments.Count(e => e.CourseId == courseId);

            if (already)
            {
                TempData["Message"] = "You are already enrolled in this course.";
                return RedirectToAction("Browse");
            }

            if (currentCount >= course.EnrollmentLimit)
            {
                TempData["Message"] = "Enrollment limit has been reached.";
                return RedirectToAction("Browse");
            }

            if (!string.IsNullOrWhiteSpace(course.PrerequisiteCourseCode))
            {
                bool passedPrerequisite = db.Enrollments.Any(e => e.StudentId == userId &&
                                                                  e.Course.CourseCode == course.PrerequisiteCourseCode &&
                                                                  e.FinalGrade >= 50);
                if (!passedPrerequisite)
                {
                    TempData["Message"] = "Prerequisite validation failed.";
                    return RedirectToAction("Browse");
                }
            }

            db.Enrollments.Add(new Enrollment
            {
                StudentId = userId,
                CourseId = courseId,
                EnrolledAt = DateTime.Now
            });
            db.SaveChanges();
            TempData["Message"] = "Enrollment successful.";
            return RedirectToAction("MyCourses");
        }
    }
}
