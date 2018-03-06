using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FH_Kiel_Ticketing_App.Context;
using FH_Kiel_Ticketing_App.Models;

namespace FH_Kiel_Ticketing_App.Controllers
{
    public class RoleIdentifiersController : Controller
    {
        private TicketingApp db = new TicketingApp();

        // GET: RoleIdentifiers
        public ActionResult Index()
        {
            return View(db.RoleIdentifier.ToList());
        }

        // GET: RoleIdentifiers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoleIdentifier roleIdentifier = db.RoleIdentifier.Find(id);
            if (roleIdentifier == null)
            {
                return HttpNotFound();
            }
            return View(roleIdentifier);
        }

        // GET: RoleIdentifiers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RoleIdentifiers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "roleIdentifierID,role")] RoleIdentifier roleIdentifier)
        {
            if (ModelState.IsValid)
            {
                db.RoleIdentifier.Add(roleIdentifier);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(roleIdentifier);
        }

        // GET: RoleIdentifiers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoleIdentifier roleIdentifier = db.RoleIdentifier.Find(id);
            if (roleIdentifier == null)
            {
                return HttpNotFound();
            }
            return View(roleIdentifier);
        }

        // POST: RoleIdentifiers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "roleIdentifierID,role")] RoleIdentifier roleIdentifier)
        {
            if (ModelState.IsValid)
            {
                db.Entry(roleIdentifier).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(roleIdentifier);
        }

        // GET: RoleIdentifiers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoleIdentifier roleIdentifier = db.RoleIdentifier.Find(id);
            if (roleIdentifier == null)
            {
                return HttpNotFound();
            }
            return View(roleIdentifier);
        }

        // POST: RoleIdentifiers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RoleIdentifier roleIdentifier = db.RoleIdentifier.Find(id);
            db.RoleIdentifier.Remove(roleIdentifier);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
