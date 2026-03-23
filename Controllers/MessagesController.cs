using System;
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
            var data = db.Messages.Where(m => m.ToUserId == userId).OrderByDescending(m => m.SentAt).ToList();
            return View(data);
        }

        [HttpGet]
        public ActionResult Compose()
        {
            int userId = (int)Session["UserId"];
            ViewBag.ToUserId = new SelectList(db.AppUsers.Where(u => u.Id != userId), "Id", "FullName");
            return View();
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
                ViewBag.ToUserId = new SelectList(db.AppUsers.Where(u => u.Id != userId), "Id", "FullName", model.ToUserId);
                return View(model);
            }

            db.Messages.Add(model);
            db.SaveChanges();
            return RedirectToAction("Inbox");
        }
    }
}
