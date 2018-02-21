using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FH_Kiel_Ticketing_App.Controllers
{
    public class ProjectRegistrationController : Controller
    {
        // GET: ProjectRegistration
        public ActionResult Index()
        {
            return View();
        }

        // GET: ProjectRegistration/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProjectRegistration/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProjectRegistration/Create
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

        // GET: ProjectRegistration/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProjectRegistration/Edit/5
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

        // GET: ProjectRegistration/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProjectRegistration/Delete/5
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
    }
}
