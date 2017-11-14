using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MobileCRM.Models;
using System.Data.Entity;

namespace MobileCRM.Controllers
{
    public class HomeController : Controller
    {
        CreditContext db = new CreditContext();

        public ActionResult Index()
        {
            return RedirectToAction("Addcreditprofile", "Home"); ;
        }

        public ActionResult Addcreditprofile()
        {
         /* if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            return RedirectToAction("Login", "Account"); */

            return View();
        }

        [HttpPost]
        public ActionResult Addcreditprofile(Credit credit)
        {
            int id = db.Credits.Add(credit).Id;
            string profile = "Creditprofile/" + id.ToString();
            db.Stories.Add(new Story { Date = DateTime.Now, User = "Галимарданов Фаузат", Action = "Добавлен новый клиент", CreditId = id });
            db.SaveChanges();
            return RedirectToAction(profile, "Home");
        }

        public ActionResult Creditlist()
        {
         /* if (User.Identity.IsAuthenticated)
            {
                return View(db.Credits.ToList());
            }
            return RedirectToAction("Login", "Account");*/

            return View(db.Credits.ToList());
        }

        public ActionResult Creditprofile(int? id)
        {
        /*  if (User.Identity.IsAuthenticated)
            {
                if (id == null)
                {
                    return HttpNotFound();
                }
                Credit credit = db.Credits.Find(id);
                //должен находить истории и комментарии привязанные к делу 
                if (credit != null)
                {
                    //ViewBag.Stories = ;
                    //ViewBag.Comments = ;
                    return View(credit);
                }
            }
            return RedirectToAction("Login", "Account"); */

            if (id == null)
            {
                return HttpNotFound();
            }
            Credit credit = db.Credits.Find(id);
            ViewBag.Credit = credit;
            ViewBag.Comments = db.Comments.Where(t => t.CreditId == id).ToList();
            ViewBag.Stories = db.Stories.Where(t => t.CreditId == id).ToList();
            if (credit != null)
            {
                return View();
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult Creditprofile(Credit credit)
        {
            db.Entry(credit).State = EntityState.Modified;
            db.SaveChanges();
            //сообщение о сохранении 
            return View();
        }

        public ActionResult Logoff()
        {
            if (User.Identity.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
            }
            return RedirectToAction("Login", "Account");
        }

        public ActionResult AddStory()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddStory(Story story)
        {
            db.Stories.Add(story);
            db.SaveChanges();
            string location = "Creditprofile/" + story.CreditId.ToString();
            return RedirectToAction( location, "Home");
        }

        public ActionResult AddComment()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddComment(Comment comment)
        {
            db.Comments.Add(comment);
            db.SaveChanges();
            string location = "Creditprofile/" + comment.CreditId.ToString();
            return RedirectToAction( location, "Home");
        }
    }
}