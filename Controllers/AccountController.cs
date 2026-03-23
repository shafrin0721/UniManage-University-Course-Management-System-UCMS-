using System;
using System.Linq;
using System.Web.Mvc;
using UniManage.DAL;
using UniManage.Helpers;
using UniManage.Models;

namespace UniManage.Controllers
{
    public class AccountController : Controller
    {
        private readonly UcmsDbContext db = new UcmsDbContext();

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            if (db.AppUsers.Any(u => u.Username == model.Username))
                ModelState.AddModelError("Username", "Username already exists.");

            if (db.AppUsers.Any(u => u.Email == model.Email))
                ModelState.AddModelError("Email", "Email already exists.");

            if (model.Role != "Student" && model.Role != "Lecturer")
                ModelState.AddModelError("Role", "Only Student and Lecturer self-registration is allowed.");

            if (!ModelState.IsValid) return View(model);

            string salt;
            var user = new AppUser
            {
                FullName = model.FullName,
                Username = model.Username,
                Email = model.Email,
                Role = model.Role,
                PasswordHash = PasswordHelper.HashPassword(model.Password, out salt),
                PasswordSalt = salt,
                CreatedAt = DateTime.Now
            };

            db.AppUsers.Add(user);
            db.SaveChanges();

            TempData["Success"] = "Registration successful. Please log in.";
            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = db.AppUsers.FirstOrDefault(u => u.Username == model.UsernameOrEmail || u.Email == model.UsernameOrEmail);

            if (user == null || !PasswordHelper.VerifyPassword(model.Password, user.PasswordSalt, user.PasswordHash))
            {
                ModelState.AddModelError("", "Invalid username/email or password.");
                return View(model);
            }

            SessionHelper.SetUserSession(Session, user.Id, user.FullName, user.Role);

            if (user.Role == "Student") return RedirectToAction("Dashboard", "Student");
            if (user.Role == "Lecturer") return RedirectToAction("Dashboard", "Lecturer");
            return RedirectToAction("Dashboard", "Admin");
        }

        public ActionResult Logout()
        {
            SessionHelper.ClearSession(Session);
            return RedirectToAction("Login");
        }
    }
}
