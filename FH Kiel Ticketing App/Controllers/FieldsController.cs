using FH_Kiel_Ticketing_App.Context;
using FH_Kiel_Ticketing_App.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FH_Kiel_Ticketing_App.Controllers
{
    public class FieldsController : Controller
    {
        TicketingApp db = new TicketingApp();
        // GET: Fields
        public ActionResult Index()
        {
            var fields = db.Fields.ToList();
            return View(fields);
        }

        // GET: Fields/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Fields/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Fields/Create
        [HttpPost]
        public ActionResult Create(Fields Fields)
        {
            try
            {
                // TODO: Add insert logic here
                db.Fields.Add(Fields);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Fields/Edit/5
        public ActionResult Edit(int id)
        {
            var field = db.Fields.Where(f => f.recordID == id).FirstOrDefault();
            return View(field);
        }

        // POST: Fields/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Fields fields)
        {
            try
            {
                // TODO: Add update logic here
                db.Entry(fields).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Fields/Delete/5
        public ActionResult Delete(int id)
        {
            var field = db.Fields.Where(f => f.recordID == id).FirstOrDefault();
            return View(field);
        }

        // POST: Fields/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Fields Fields)
        {
            try
            {
                // TODO: Add delete logic here
                var field = db.Fields.Where(f => f.recordID == id).FirstOrDefault();
                db.Fields.Remove(field);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
