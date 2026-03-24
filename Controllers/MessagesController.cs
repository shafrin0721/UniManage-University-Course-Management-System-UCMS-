using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using UniManage.DAL;
using UniManage.Filters;
using UniManage.Models;

namespace UniManage.Controllers
{
    [AuthorizeRole("Student", "Lecturer", "Administrator")]
    public class MessagesController : Controller
    {
        private readonly UcmsDbContext db = new UcmsDbContext();

        public ActionResult Inbox()
        {
            int userId = (int)Session["UserId"];
            var data = db.Messages
                .Where(m => m.ToUserId == userId)
                .OrderByDescending(m => m.SentAt)
                .ToList();

            ViewBag.UserNames = db.AppUsers
                .ToDictionary(u => u.Id, u => u.FullName);

            return View(data);
        }

        public ActionResult Sent()
        {
            int userId = (int)Session["UserId"];
            var data = db.Messages
                .Where(m => m.FromUserId == userId)
                .OrderByDescending(m => m.SentAt)
                .ToList();

            ViewBag.UserNames = db.AppUsers
                .ToDictionary(u => u.Id, u => u.FullName);

            return View(data);
        }

        public ActionResult MarkAsRead(int id)
        {
            var message = db.Messages.Find(id);
            if (message != null)
            {
                message.IsRead = true;
                db.SaveChanges();
            }
            return RedirectToAction("Inbox");
        }

        [HttpGet]
        public ActionResult Compose(int? replyToUserId = null, string replySubject = null)
        {
            int userId = (int)Session["UserId"];
            ViewBag.ToUserId = new SelectList(
                db.AppUsers.Where(u => u.Id != userId), "Id", "FullName");

            if (replyToUserId.HasValue)
            {
                ViewBag.ReplyToUserId = replyToUserId.Value;
                ViewBag.ReplySubject = replySubject;
            }

            return View(new Message());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Compose(Message model)
        {
            model.FromUserId = (int)Session["UserId"];
            model.SentAt = DateTime.Now;
            model.IsRead = false;

            if (!ModelState.IsValid)
            {
                int userId = (int)Session["UserId"];
                ViewBag.ToUserId = new SelectList(
                    db.AppUsers.Where(u => u.Id != userId),
                    "Id", "FullName", model.ToUserId);
                return View(model);
            }

            db.Messages.Add(model);
            db.SaveChanges();

            TempData["Success"] = "Message sent successfully.";
            return RedirectToAction("Inbox");
        }
    }
}