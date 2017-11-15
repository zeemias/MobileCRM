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
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            return RedirectToAction("Login", "Account"); 
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
            if (User.Identity.IsAuthenticated)
            {
                return View(db.Credits.ToList());
            }
            return RedirectToAction("Login", "Account");
        }

        public ActionResult Creditprofile(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
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
            return RedirectToAction("Login", "Account");
        }

        /*[HttpPost]
        public ActionResult Creditprofile(Credit credit)
        {
            db.Entry(credit).State = EntityState.Modified;
            db.SaveChanges();
            string location = "Creditprofile/" + credit.Id;
            //сообщение о сохранении 
            return RedirectToAction( location, "Home");
        }*/

        [HttpPost]
        public ActionResult Creditprofile()
        {
            if (Request.IsAjaxRequest())
            {
                int id = Convert.ToInt32(Request["CreditId"]);
                switch (Request["Type"])
                {
                    case "comment":
                        Comment comment = new Comment()
                        {
                            CreditId = id,
                            Date = DateTime.Now,
                            User = "Галимарданов Фаузат",
                            UserComment = Request["UserComment"]
                        };
                        db.Comments.Add(comment);
                        db.SaveChanges();
                        ViewBag.Credit = db.Credits.Find(id);
                        ViewBag.Comments = db.Comments.Where(t => t.CreditId == id).ToList();
                        return PartialView("AddComment");
                    case "story":
                        Story story = new Story()
                        {
                            CreditId = id,
                            Date = DateTime.Now,
                            User = "Галимарданов Фаузат",
                            Action = Request["Action"]
                        };
                        db.Stories.Add(story);
                        db.SaveChanges();
                        ViewBag.Credit = db.Credits.Find(id);
                        ViewBag.Stories = db.Stories.Where(t => t.CreditId == id).ToList();
                        return PartialView("AddStory");
                    case "save":
                        break;
                    case "photo":
                        break;
                    default:
                        return HttpNotFound();
                }
            }
       
            return View();
        }

        public ActionResult AddStory()
        {
            return View();
        }

        public ActionResult AddComment()
        {
            return View();
        }

        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase upload)
        {
            string fileName;
            if (upload != null)
            {
                fileName = System.IO.Path.GetFileName(upload.FileName);
                upload.SaveAs(Server.MapPath("~/Content/Clients/" + fileName));
                fileName = "~/Content/Clients/" + fileName;
            }
            return RedirectToAction("Addcreditprofile");
        }

        public ActionResult EditPhoto()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EditPhoto(HttpPostedFileBase upload)
        {
            string fileName;
            if (upload != null)
            {
                fileName = System.IO.Path.GetFileName(upload.FileName);
                upload.SaveAs(Server.MapPath("~/Content/Clients/" + fileName));
                fileName = "~/Content/Clients/" + fileName;
            }
            return RedirectToAction("Addcreditprofile");
        }

        public ActionResult AjaxTest()
        {
            if (Request.IsAjaxRequest())
            {
                ViewData["data"] = Request["data"];
                return PartialView("AjaxTestPartial");
            }

            return View();
        }

        public ActionResult AjaxTestPartial()
        {
            return View();
        }

    }
}