using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MobileCRM.Models;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace MobileCRM.Controllers
{
    public class HomeController : Controller
    {

        CreditContext db = new CreditContext();
        public string photoPath;

        public ActionResult Index()
        {
            throw new Exception("Тест");
            return RedirectToAction("Addcreditprofile", "Home"); ;
        }

        public ActionResult Addcreditprofile()
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.Role = db.Users.Where(t => t.Login == User.Identity.Name).FirstOrDefault().Role;
                return View();
            }
            return RedirectToAction("Login", "Account"); 
        }

        [HttpPost]
        public ActionResult Addcreditprofile(Credit credit)
        {
            credit.UserLogin = User.Identity.Name;
            db.Credits.Add(credit);
            db.SaveChanges();
            int id = credit.Id;
            User user = db.Users.Where(t => t.Login == User.Identity.Name).FirstOrDefault();
            db.Stories.Add(new Story { Date = DateTime.Now, User =  user.Surname + " " + user.Name, Action = "Добавлен новый клиент", CreditId = id });
            db.SaveChanges();
            string profile = "Creditprofile/" + id.ToString();
            return RedirectToAction(profile, "Home");
        }

        public ActionResult Creditlist(int page = 1)
        {
            if (User.Identity.IsAuthenticated)
            {
                int pageSize = 10;
                List<Credit> credits;
                if (db.Users.Where(t=>t.Login == User.Identity.Name).FirstOrDefault().Role == "Пользователь")
                {
                    credits = db.Credits.Where(t=>t.UserLogin == User.Identity.Name).OrderBy(t => t.Surname).ThenBy(t => t.Name).ThenBy(t => t.Patronymic).ToList();
                }
                else
                {
                    credits = db.Credits.OrderBy(t => t.Surname).ThenBy(t => t.Name).ThenBy(t => t.Patronymic).ToList();
                }
                IEnumerable<Credit> creditsPerPages = credits.Skip((page - 1) * pageSize).Take(pageSize);
                PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = credits.Count };
                IndexViewModel ivm = new IndexViewModel { PageInfo = pageInfo, Credits = creditsPerPages };
                ViewBag.Role = db.Users.Where(t => t.Login == User.Identity.Name).FirstOrDefault().Role;
                return View(ivm);
            }
            return RedirectToAction("Login", "Account");
        }

        public ActionResult Userlist(int page = 1)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (db.Users.Where(t => t.Login == User.Identity.Name).FirstOrDefault().Role == "Пользователь")
                {
                    return RedirectToAction("Http404", "Error");
                }
                else
                {
                    int pageSize = 10;
                    List<User> users = db.Users.OrderBy(t => t.Surname).ThenBy(t => t.Name).ThenBy(t => t.Patronymic).ToList();
                    IEnumerable<User> usersPerPages = users.Skip((page - 1) * pageSize).Take(pageSize);
                    PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = users.Count };
                    IndexViewModel ivm = new IndexViewModel { PageInfo = pageInfo, Users = usersPerPages };
                    ViewBag.Role = db.Users.Where(t => t.Login == User.Identity.Name).FirstOrDefault().Role;
                    return View(ivm);
                }
            }
            return RedirectToAction("Login", "Account");
        }

        public ActionResult Errorlist(int page = 1)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (db.Users.Where(t => t.Login == User.Identity.Name).FirstOrDefault().Role == "Программист")
                {
                    int pageSize = 10;
                    List<Error> errors = db.Errors.OrderBy(t => t.Date).ToList();
                    IEnumerable<Error> errorsPerPages = errors.Skip((page - 1) * pageSize).Take(pageSize);
                    PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = errors.Count };
                    IndexViewModel ivm = new IndexViewModel { PageInfo = pageInfo, Errors = errorsPerPages };
                    ViewBag.Role = db.Users.Where(t => t.Login == User.Identity.Name).FirstOrDefault().Role;
                    return View(ivm);
                }
                else
                {
                    return RedirectToAction("Http404", "Error");
                }
                
            }
            return RedirectToAction("Login", "Account");
        }

        public ActionResult Creditprofile(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (id == null)
                {
                    return RedirectToAction("Http404", "Error");
                }
                Credit credit = db.Credits.Find(id);
                ViewBag.Credit = credit;
                ViewBag.Comments = db.Comments.Where(t => t.CreditId == id).ToList();
                ViewBag.Stories = db.Stories.Where(t => t.CreditId == id).ToList();
                if (credit != null)
                {
                    ViewBag.Role = db.Users.Where(t => t.Login == User.Identity.Name).FirstOrDefault().Role;
                    return View();
                }
                return RedirectToAction("Http404", "Error");
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public ActionResult Creditprofile()
        {
            if (Request.IsAjaxRequest())
            {
                User user = db.Users.Where(t => t.Login == User.Identity.Name).FirstOrDefault();
                int idAdd = Convert.ToInt32(Request["CreditId"]);
                int idEdit = Convert.ToInt32(Request["Id"]);
                switch (Request["Type"])
                {
                    case "comment":
                        Comment comment = new Comment()
                        {
                            CreditId = idAdd,
                            Date = DateTime.Now,
                            User = user.Surname + " " + user.Name,
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
                            User = user.Surname + " " + user.Name,
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
                            UserLogin = User.Identity.Name,
                            PhoneNumber = Convert.ToInt64(Request["PhoneNumber"]),
                            Birthday = Convert.ToDateTime(Request["Birthday"])
                        };
                        db.Entry(credit).State = EntityState.Modified;
                        db.SaveChanges();
                        ViewBag.Credit = db.Credits.Find(idEdit);
                        return PartialView("EditProfile");
                    default:
                        return RedirectToAction("Http404", "Error");
                }
            }
            ViewBag.Role = db.Users.Where(t => t.Login == User.Identity.Name).FirstOrDefault().Role;
            return View();
        }

        public ActionResult AddStory()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            return RedirectToAction("Login", "Account");
        }

        public ActionResult AddComment()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            return RedirectToAction("Login", "Account");
        }

        public ActionResult AddProfile(Credit credit)
        {
            if (User.Identity.IsAuthenticated)
            {
                credit.UserLogin = User.Identity.Name;
                db.Credits.Add(credit);
                db.SaveChanges();
                int id = credit.Id;
                User user = db.Users.Where(t => t.Login == User.Identity.Name).FirstOrDefault();
                db.Stories.Add(new Story { Date = DateTime.Now, User = user.Surname + " " + user.Name, Action = "Добавлен новый клиент", CreditId = id });
                db.SaveChanges();
                string profile = "Creditprofile/" + id.ToString();
                return RedirectToAction(profile, "Home");
            }
            return RedirectToAction("Login", "Account");
        }

        public ActionResult EditProfile()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            return RedirectToAction("Login", "Account");
        }

        public ActionResult Userprofile(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (db.Users.Where(t => t.Login == User.Identity.Name).FirstOrDefault().Role == "Пользователь")
                {
                    if (id == null)
                    {
                        User user = db.Users.Where(t => t.Login == User.Identity.Name).FirstOrDefault();
                        ViewBag.User = user;
                        ViewBag.Role = db.Users.Where(t => t.Login == User.Identity.Name).FirstOrDefault().Role;
                    }
                    else
                    {
                        return RedirectToAction("Http404", "Error");
                    }
                    return View();
                }
                else
                {
                    if (id == null)
                    {
                        User user = db.Users.Where(t => t.Login == User.Identity.Name).FirstOrDefault();
                        ViewBag.User = user;
                        ViewBag.Role = db.Users.Where(t => t.Login == User.Identity.Name).FirstOrDefault().Role;
                    }
                    else
                    {
                        ViewBag.User = db.Users.Find(id);
                        if (ViewBag.User == null)
                        {
                            return RedirectToAction("Http404", "Error");
                        }
                        ViewBag.Role = db.Users.Where(t => t.Login == User.Identity.Name).FirstOrDefault().Role;
                    }
                    return View();
                }
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public ActionResult Userprofile(int Id, string Photo, string Surname, string Name, string Patronymic, string Password)
        {
            User user = db.Users.Find(Id);
            user.Name = Name;
            user.Surname = Surname;
            user.Patronymic = Patronymic;
            user.Photo = Photo;
            if (Password != null)
            {
                Password = getMd5Hash(Password);
                if(Password != user.Password)
                {
                    user.Password = Password;
                }
            }
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
            string path = "Userprofile/" + user.Id;
            return RedirectToAction(path, "Home");
        }

        public ActionResult Error(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (db.Users.Where(t => t.Login == User.Identity.Name).FirstOrDefault().Role == "Программист")
                {
                    if (id == null)
                    {
                        return RedirectToAction("Http404", "Error");
                    }
                    Error error = db.Errors.Find(id);
                    ViewBag.Error = error;
                    ViewBag.Role = db.Users.Where(t => t.Login == User.Identity.Name).FirstOrDefault().Role;
                    return View();
                }
                else
                {
                    return RedirectToAction("Http404", "Error");
                }
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public ActionResult Error(string Status, int Id)
        {
            Error error = db.Errors.Find(Id);
            error.Status = Status;
            db.Entry(error).State = EntityState.Modified;
            db.SaveChanges();
            string path = "Error/" + Id;
            return RedirectToAction(path, "Home");
        }

        public JsonResult UploadPhoto()
        {
            foreach (string file in Request.Files)
            {
                var upload = Request.Files[file];
                if (upload != null)
                {
                    string fileName = "~/Content/Clients/" + System.IO.Path.GetFileName(upload.FileName);
                    upload.SaveAs(Server.MapPath(fileName));
                    photoPath = fileName;
                }
            }
            return Json(photoPath);
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