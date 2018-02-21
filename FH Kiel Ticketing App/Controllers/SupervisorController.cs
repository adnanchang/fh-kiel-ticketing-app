using FH_Kiel_Ticketing_App.Context;
using FH_Kiel_Ticketing_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FH_Kiel_Ticketing_App.Controllers
{
    public class SupervisorController : Controller
    {
        TicketingApp db = new TicketingApp();
        // GET: Supervisor
        public ActionResult Index()
        {
            if (IsLoggedIn() && IsAuthorized())
            {
                int userID = GetUserID();
                var user = db.User.Where(u => u.recordID == userID).FirstOrDefault();
                var supervisor = db.Supervisor.Where(s => s.recordID == userID).FirstOrDefault();
                var tickets = db.Ticket.Where(t => t.recordID > 0).ToList();
                var ticket = db.Ticket.Where(t => t.recordID > 0).FirstOrDefault();
                var idea = db.Idea.Where(i => i.User.recordID != userID).ToList();

                var sprTicketView = new SuperVisorTicketViewModel
                {
                    user = user,
                    supervisor = supervisor,
                    availableIdeas = idea,
                    ticket = ticket,
                    tickets = tickets
                };
                return View(sprTicketView);
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }

        // GET: Supervisor/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Supervisor/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Supervisor/Create
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

        // GET: Supervisor/Edit/5
        public ActionResult Edit(int? id)
        {
            if (IsLoggedIn() && IsAuthorized())
            {
                var user = db.User.Where(u => u.recordID == id).FirstOrDefault();
                var supervisor = db.Supervisor.Where(s => s.recordID == id).FirstOrDefault();
                var fields = db.Fields.ToList();
                var supervisorFields = db.Fields.Where(f => f.Supervisor.Any(s => s.recordID == id)).ToList();
                var selectedFields = supervisorFields.Select(x => x.recordID).ToArray();

                var supervisorUser = new SupervisorUserFieldViewModel
                {
                    user = user,
                    supervisor = supervisor,
                    SupervisorFields = supervisorFields,
                    AllFields = fields,
                    selectedFields = selectedFields
                };
                return View(supervisorUser);
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }

        // POST: Supervisor/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, SupervisorUserFieldViewModel supervisorUser)
        {
            try
            {
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    using (TicketingApp db = new TicketingApp())
                    {
                        var supervisor = db.Supervisor.Where(s => s.recordID == supervisorUser.supervisor.recordID).FirstOrDefault();
                        var fields = new List<Fields>();
                        foreach (int item in supervisorUser.selectedFields)
                        {
                            fields.Add(db.Fields.Where(f => f.recordID == item).FirstOrDefault());
                        }
                        supervisor.Fields.Clear();
                        foreach (var item in fields)
                        {
                            supervisor.Fields.Add(item);
                        }
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

        // GET: Supervisor/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Supervisor/Delete/5
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
                if (Request.Cookies["UserCookie"]["UserRole"].ToString() == "Supervisor")
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
