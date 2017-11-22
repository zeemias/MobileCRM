using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using MobileCRM.Models;
using System.Security.Cryptography;
using System.Text;

namespace MobileCRM.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Addcreditprofile", "Home");
            }
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
                    string pass = getMd5Hash(model.Password);
                    user = db.Users.FirstOrDefault(u => u.Login == model.Login && u.Password == pass);

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
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Addcreditprofile", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
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
                    string pass = getMd5Hash(model.Password);
                    db.Users.Add(new User { Login = model.Login, Name = model.Name, Surname = model.Surname, Patronymic = model.Patronymic, Password = pass, Role = "Пользователь", Photo = "~/Content/img/avatar.png" });
                    db.SaveChanges();
                    user = db.Users.FirstOrDefault(u => u.Login == model.Login && u.Password == pass);
                }
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(model.Login, true);
                    return RedirectToAction("Addcreditprofile", "Home");
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

        public string getMd5Hash(string input)
        {
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
    }
}