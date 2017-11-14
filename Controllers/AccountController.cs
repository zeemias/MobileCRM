using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using MobileCRM.Models;

namespace MobileCRM.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User user = null;
                using (CreditContext db = new CreditContext())
                {
                    user = db.Users.FirstOrDefault(u => u.Login == model.Login && u.Password == model.Password);

                }
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(model.Login, true);
                    return RedirectToAction("Addcreditprofile", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин или пароль");
                }
            }

            return View(model);
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = null;
                using (CreditContext db = new CreditContext())
                {
                    user = db.Users.FirstOrDefault(u => u.Login == model.Login);
                }
                if (user == null)
                {
                    using (CreditContext db = new CreditContext())
                    {
                        db.Users.Add(new User { Login = model.Login, Name = model.Name, Surname = model.Surname, Patronymic = model.Patronymic, Password = model.Password });
                        db.SaveChanges();

                        user = db.Users.Where(u => u.Login == model.Login && u.Password == model.Password).FirstOrDefault();
                    }
                    if (user != null)
                    {
                        FormsAuthentication.SetAuthCookie(model.Login, true);
                        return RedirectToAction("Addcreditprofile", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Пользователь с таким логином уже существует");
                }
            }

            return View(model);
        }

        public ActionResult Logoff()
        {
            if (User.Identity.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
            }
            return RedirectToAction("Login", "Account");
        }
    }
}