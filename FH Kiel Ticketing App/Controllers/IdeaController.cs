using FH_Kiel_Ticketing_App.Context;
using FH_Kiel_Ticketing_App.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FH_Kiel_Ticketing_App.Controllers
{
    public class IdeaController : Controller
    {
        TicketingApp db = new TicketingApp();
        // GET: Idea
        public ActionResult Index(int? page)
        {
            if (IsLoggedIn())
            {
                string userRole = GetUserRole();
                int userId = GetUserID();
                var user = db.User.Where(u => u.recordID == userId).FirstOrDefault();
                IdeaListViewModel ideaList = null;
                int pageSize = 7;
                int pageNumber = (page ?? 1);
                if (userRole == "Student")
                {
                    var ideas = db.Idea.Where(i => i.User.recordID != userId).ToList();

                    ideaList = new IdeaListViewModel
                    {
                        user = user,
                        ideas = ideas.ToPagedList(pageNumber, pageSize)
                    };
                }
                else if (userRole == "Supervisor")
                {
                    var ideas = db.Idea.Where(i => i.User.recordID == userId).ToList();
                    ideaList = new IdeaListViewModel
                    {
                        user = user,
                        ideas = ideas.ToPagedList(pageNumber, pageSize)
                    };
                }
                return View(ideaList);
            }
            return RedirectToAction("Login", "User");
        }

        // GET: Idea/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Login", "User");
            }

            if (IsLoggedIn())
            {
                int userId = GetUserID();
                var user = db.User.Where(u => u.recordID == userId).FirstOrDefault();
                var idea = db.Idea.Where(i => i.recordID == id).FirstOrDefault();

                int fieldID = Convert.ToInt32(idea.field);
                var field = db.Fields.Where(f => f.recordID == fieldID).FirstOrDefault();
                idea.field = field.field;
                var ideaDetails = new IdeaDetailsViewModel
                {
                    user = user,
                    Idea = idea
                };
                return View(ideaDetails);
            }
            else
            {
                return RedirectToAction("Login", "User");
            }

        }

        // GET: Idea/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Idea/Create
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

        // GET: Idea/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Idea/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Idea/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Idea/Delete/5
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
