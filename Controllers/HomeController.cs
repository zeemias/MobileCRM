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
using System.Text.RegularExpressions;

namespace MobileCRM.Controllers
{
    public class HomeController : Controller
    {

        CreditContext db = new CreditContext();
        public string photoPath;

        public ActionResult Index()
        {
            //throw new Exception("Тест");
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
                ViewBag.Id = credit.Id;
                ViewBag.Credit = credit;
                ViewBag.Comments = db.Comments.Where(t => t.CreditId == id).ToList();
                ViewBag.Stories = db.Stories.Where(t => t.CreditId == id).ToList();
                if (credit != null)
                {
                    ViewBag.Role = db.Users.Where(t => t.Login == User.Identity.Name).FirstOrDefault().Role;
                    return View(credit);
                }
                return RedirectToAction("Http404", "Error");
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public ActionResult Creditprofile(Credit credit)
        {
            if (Request.IsAjaxRequest())
            {
                User user = db.Users.Where(t => t.Login == User.Identity.Name).FirstOrDefault();
                int idAdd = Convert.ToInt32(Request["CreditId"]);
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
                        ViewBag.Id = idAdd;
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
                        ViewBag.Id = idAdd;
                        ViewBag.Stories = db.Stories.Where(t => t.CreditId == idAdd).ToList();
                        return PartialView("AddStory");
                    default:
                        return RedirectToAction("Http404", "Error");
                }
            }
            if(credit != null)
            {
                credit.UserLogin = User.Identity.Name;
                db.Entry(credit).State = EntityState.Modified;
                db.SaveChanges();
                string path = "Creditprofile/" + credit.Id.ToString();
                return RedirectToAction(path, "Home");
            }
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
                        ViewBag.Role = db.Users.Where(t => t.Login == User.Identity.Name).FirstOrDefault().Role;
                        return View(user);
                    }
                    else
                    {
                        return RedirectToAction("Http404", "Error");
                    }
                }
                else
                {
                    if (id == null)
                    {
                        User user = db.Users.Where(t => t.Login == User.Identity.Name).FirstOrDefault();
                        ViewBag.Role = db.Users.Where(t => t.Login == User.Identity.Name).FirstOrDefault().Role;
                        return View(user);
                    }
                    else
                    {
                        User user = db.Users.Find(id);
                        if (user == null)
                        {
                            return RedirectToAction("Http404", "Error");
                        }
                        ViewBag.Role = db.Users.Where(t => t.Login == User.Identity.Name).FirstOrDefault().Role;
                        return View(user);
                    }
                }
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public ActionResult Userprofile(User user)
        {
            User userProfile = db.Users.Find(user.Id);
            string userPassword = userProfile.Password;
            if (user.Password != null)
            {
                string Password = getMd5Hash(user.Password);
                if(userPassword != Password)
                {
                    user.Password = Password;
                }
                else
                {
                    user.Password = userPassword;
                }
            }
            else
            {
                user.Password = userPassword;
            }
            db.Entry(userProfile).CurrentValues.SetValues(user);
            db.Entry(userProfile).State = EntityState.Modified;
            db.SaveChanges();
            if (User.Identity.Name == user.Login)
            {
                return RedirectToAction("Userprofile", "Home");
            }
            else
            {
                string path = "Userprofile/" + user.Id;
                return RedirectToAction(path, "Home");
            }
            
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

        public JsonResult ValidateLogin(string Login)
        {
            User user  = db.Users.FirstOrDefault(t => t.Login == Login);
            if (user != null)
            {
                return Json("Данный логин занят", JsonRequestBehavior.AllowGet);
            } else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ValidateEmail(string Email)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(Email);
            if (match.Success)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Неправильный Email", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ValidatePhone(string PhoneNumber)
        {
            if(PhoneNumber != null)
            {
                Regex regex = new Regex(@"^((\+7|8)+([0-9]){10})$");
                Match match = regex.Match(PhoneNumber);
                if (match.Success)
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Неправильный номер телефона", JsonRequestBehavior.AllowGet);
                }
            }
            return Json("Неправильный номер телефона", JsonRequestBehavior.AllowGet);
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