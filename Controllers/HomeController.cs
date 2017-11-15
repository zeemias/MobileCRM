﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MobileCRM.Models;
using System.Data.Entity;
using System.Globalization;

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
        public ActionResult Addcreditprofile(string photo)
        {
            if (Request.IsAjaxRequest())
            {
                HttpPostedFileBase upload = Request.Files["upload"];
                string fileName;
                if (upload != null && upload.ContentLength > 0 && !string.IsNullOrEmpty(upload.FileName))
                {
                    fileName = upload.FileName; ;
                    upload.SaveAs(Server.MapPath("~/Content/Clients/" + fileName));
                    fileName = "~/Content/Clients/" + fileName;
                    ViewBag.Photo = fileName;
                }
                return PartialView("Upload");
            }
            return View();
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

        [HttpPost]
        public ActionResult Creditprofile()
        {
            if (Request.IsAjaxRequest())
            {
                int idAdd = Convert.ToInt32(Request["CreditId"]);
                int idEdit = Convert.ToInt32(Request["Id"]);
                throw new Exception("123");
                switch (Request["Type"])
                {
                    case "comment":
                        Comment comment = new Comment()
                        {
                            CreditId = idAdd,
                            Date = DateTime.Now,
                            User = "Галимарданов Фаузат",
                            UserComment = Request["UserComment"]
                        };
                        db.Comments.Add(comment);
                        db.SaveChanges();
                        ViewBag.Credit = db.Credits.Find(idAdd);
                        ViewBag.Comments = db.Comments.Where(t => t.CreditId == idAdd).ToList();
                        return PartialView("AddComment");
                    case "story":
                        Story story = new Story()
                        {
                            CreditId = idAdd,
                            Date = DateTime.Now,
                            User = "Галимарданов Фаузат",
                            Action = Request["Action"]
                        };
                        db.Stories.Add(story);
                        db.SaveChanges();
                        ViewBag.Credit = db.Credits.Find(idAdd);
                        ViewBag.Stories = db.Stories.Where(t => t.CreditId == idAdd).ToList();
                        return PartialView("AddStory");
                    case "profile":
                        Credit credit = new Credit()
                        {
                            Id = idEdit,
                            Name = Request["Name"],
                            Surname = Request["Surname"],
                            Patronymic = Request["Patronymic"],
                            Photo = Request["Photo"],
                            Email = Request["Email"],
                            Source = Request["Source"],
                            Work = Request["Work"],
                            PhoneNumber = Convert.ToInt64(Request["PhoneNumber"]),
                            Birthday = Convert.ToDateTime(Request["Birthday"])
                        };
                        db.Entry(credit).State = EntityState.Modified;
                        db.SaveChanges();
                        ViewBag.Credit = db.Credits.Find(idEdit);
                        return PartialView("EditProfile");
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

        public ActionResult AddProfile(Credit credit)
        {
            db.Credits.Add(credit);
            db.SaveChanges();
            int id = credit.Id;
            db.Stories.Add(new Story { Date = DateTime.Now, User = "Галимарданов Фаузат", Action = "Добавлен новый клиент", CreditId = id });
            db.SaveChanges();
            string profile = "Creditprofile/" + id.ToString();
            return RedirectToAction(profile, "Home");
        }

        public ActionResult EditProfile()
        {
            return View();
        }

        public ActionResult EditPhoto()
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

    }
}