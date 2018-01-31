using FH_Kiel_Ticketing_App.Context;
using FH_Kiel_Ticketing_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FH_Kiel_Ticketing_App.Controllers
{
    public class StudentController : Controller
    {
        TicketingApp db = new TicketingApp();
        // GET: Student
        public ActionResult Index()
        {
            if (IsLoggedIn() && IsAuthorized())
            {
                int userID = GetUserID();

                var user = db.User.Where(u => u.recordID == userID).FirstOrDefault();
                var student = db.Student.Where(s => s.recordID == userID).FirstOrDefault();
                var ticket = db.Ticket.Where(t => t.Contributors.Any(c => c.User.recordID == userID)).FirstOrDefault();
                var idea = db.Idea.Where(i => i.User.recordID != userID).ToList();

                var studentUser = new StudentUserViewModel
                {
                    user = user,
                    student = student,
                    ticket = ticket,
                    availableIdeas = idea
                };

                if (student.matrikelNumber == 0)
                {
                    ViewBag.IsDataSet = false;
                }

                return View(studentUser);
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }

        // GET: Student/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Student/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Student/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Student/Edit/5
        public ActionResult Edit(int? id)
        {
            if (IsLoggedIn() && IsAuthorized())
            {
                using (TicketingApp db = new TicketingApp())
                {
                    var user = db.User.Where(u => u.recordID == id).FirstOrDefault();
                    var student = db.Student.Where(s => s.recordID == id).FirstOrDefault();

                    var studentUser = new StudentUserViewModel
                    {
                        user = user,
                        student = student
                    };
                    return View(studentUser);
                }
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }

        // POST: Student/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, StudentUserViewModel studentUser)
        {
            try
            {
                // TODO: Add update logic here
                ModelState.Remove("User.firstName");
                ModelState.Remove("User.lastName");
                ModelState.Remove("User.email");
                ModelState.Remove("User.password");
                ModelState.Remove("User.confirmPassword");
                if (ModelState.IsValid)
                {
                    using (TicketingApp db = new TicketingApp())
                    {
                        db.Configuration.ValidateOnSaveEnabled = false;
                        var user = db.User.Where(u => u.recordID == studentUser.user.recordID).FirstOrDefault();
                        var student = db.Student.Where(s => s.recordID == studentUser.user.recordID).FirstOrDefault();

                        user.emailNotification = studentUser.user.emailNotification;

                        student.matrikelNumber = studentUser.student.matrikelNumber;
                        student.beginningSemesterSeason = studentUser.student.beginningSemesterSeason;
                        student.beginningSemesterYear = studentUser.student.beginningSemesterYear;

                        db.SaveChanges();
                    }
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Student/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Student/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [NonAction]
        public bool IsLoggedIn()
        {
            if (Request.Cookies["UserCookie"] != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [NonAction]
        public bool IsAuthorized()
        {
            if (Request.Cookies["UserCookie"] != null)
            {
                if (Request.Cookies["UserCookie"]["UserRole"].ToString() == "Student")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        [NonAction]
        public int GetUserID()
        {
            int userID = -1;
            if (Request.Cookies["UserCookie"] != null)
            {
                userID = Convert.ToInt32(Request.Cookies["UserCookie"]["UserID"].ToString());
            }
            return userID;
        }

        [NonAction]
        public string GetUserRole()
        {
            string userRole = null;
            if (Request.Cookies["UserCookie"] != null)
            {
                userRole = Request.Cookies["UserCookie"]["UserRole"].ToString();
            }
            return userRole;
        }
    }
}
