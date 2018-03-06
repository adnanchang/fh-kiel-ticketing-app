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
    public class RoleIdentifierDetailsController : Controller
    {
        private TicketingApp db = new TicketingApp();

        // GET: RoleIdentifierDetails
        public ActionResult Index()
        {
            var roleIdentifierDetails = db.RoleIdentifierDetails.Include(r => r.RoleIdentifier);
            return View(roleIdentifierDetails.ToList());
        }

        // GET: RoleIdentifierDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoleIdentifierDetails roleIdentifierDetails = db.RoleIdentifierDetails.Find(id);
            if (roleIdentifierDetails == null)
            {
                return HttpNotFound();
            }
            return View(roleIdentifierDetails);
        }

        // GET: RoleIdentifierDetails/Create
        public ActionResult Create()
        {
            ViewBag.roleIdentifierID = new SelectList(db.RoleIdentifier, "roleIdentifierID", "role");
            return View();
        }

        // POST: RoleIdentifierDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "recordID,identifier,roleIdentifierID")] RoleIdentifierDetails roleIdentifierDetails)
        {
            if (ModelState.IsValid)
            {
                db.RoleIdentifierDetails.Add(roleIdentifierDetails);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.roleIdentifierID = new SelectList(db.RoleIdentifier, "roleIdentifierID", "role", roleIdentifierDetails.roleIdentifierID);
            return View(roleIdentifierDetails);
        }

        // GET: RoleIdentifierDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoleIdentifierDetails roleIdentifierDetails = db.RoleIdentifierDetails.Find(id);
            if (roleIdentifierDetails == null)
            {
                return HttpNotFound();
            }
            ViewBag.roleIdentifierID = new SelectList(db.RoleIdentifier, "roleIdentifierID", "role", roleIdentifierDetails.roleIdentifierID);
            return View(roleIdentifierDetails);
        }

        // POST: RoleIdentifierDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "recordID,identifier,roleIdentifierID")] RoleIdentifierDetails roleIdentifierDetails)
        {
            if (ModelState.IsValid)
            {
                db.Entry(roleIdentifierDetails).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.roleIdentifierID = new SelectList(db.RoleIdentifier, "roleIdentifierID", "role", roleIdentifierDetails.roleIdentifierID);
            return View(roleIdentifierDetails);
        }

        // GET: RoleIdentifierDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoleIdentifierDetails roleIdentifierDetails = db.RoleIdentifierDetails.Find(id);
            if (roleIdentifierDetails == null)
            {
                return HttpNotFound();
            }
            return View(roleIdentifierDetails);
        }

        // POST: RoleIdentifierDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RoleIdentifierDetails roleIdentifierDetails = db.RoleIdentifierDetails.Find(id);
            db.RoleIdentifierDetails.Remove(roleIdentifierDetails);
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
