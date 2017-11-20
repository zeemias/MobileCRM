using MobileCRM.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MobileCRM.Controllers
{
    public class ErrorController : Controller
    {
        CreditContext db = new CreditContext();

        public ActionResult General(Exception exception)
        {
            Error error = new Error
            {
                Date = DateTime.Now,
                Message = exception.Message,
                StackTrace = exception.StackTrace,
                User = "Гость",
                Status = "Новая ошибка"
            };

            if (User.Identity.IsAuthenticated)
            {
                error.User = User.Identity.Name;
                db.Errors.Add(error);
                db.SaveChanges();
                ViewBag.Id = error.Id;
                ViewBag.Role = db.Users.Where(t => t.Login == User.Identity.Name).FirstOrDefault().Role;
                return View();
            }
            db.Errors.Add(error);
            db.SaveChanges();
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public ActionResult General(int Id, string Comment)
        {
            Error error = db.Errors.Find(Id);
            error.UserComment = Comment;
            db.Entry(error).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Success", "Error");
        }

        public ActionResult Http404()
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.Role = db.Users.Where(t => t.Login == User.Identity.Name).FirstOrDefault().Role;
                return View();
            }
            return RedirectToAction("Login", "Account");
        }

        public ActionResult Http403()
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.Role = db.Users.Where(t => t.Login == User.Identity.Name).FirstOrDefault().Role;
                return View();
            }
            return RedirectToAction("Login", "Account");
        }

        public ActionResult Success()
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.Role = db.Users.Where(t => t.Login == User.Identity.Name).FirstOrDefault().Role;
                return View();
            }
            return RedirectToAction("Login", "Account");
        }

    }
}