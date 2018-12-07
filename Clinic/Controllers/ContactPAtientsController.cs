using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Clinic;

namespace Clinic.Models.Controllers
{
    public class ContactPAtientsController : Controller
    {
        private ClinicEntities db = new ClinicEntities();

        // GET: ContactPAtients
        public ActionResult Index()
        {
            var contactPAtients = db.ContactPAtients;
            return View(contactPAtients.ToList());
        }

        // GET: ContactPAtients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactPAtient contactPAtient = db.ContactPAtients.Find(id);
            if (contactPAtient == null)
            {
                return HttpNotFound();
            }
            return View(contactPAtient);
        }

        // GET: ContactPAtients/Create
        public ActionResult Create()
        {
            ViewBag.ID_PATIENT = new SelectList(db.Patients, "ID_Patient", "nom");
            return View();
        }

        // POST: ContactPAtients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_ContactPatient,nom,prenom,date_naissance,sexe,profession,situation_familial,groupe_sanguin,email,telephone,ID_PATIENT")] ContactPAtient contactPAtient)
        {
            if (ModelState.IsValid)
            {
                db.ContactPAtients.Add(contactPAtient);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ID_PATIENT = new SelectList(db.Patients, "ID_Patient", "nom", contactPAtient.ID_PATIENT);
            return View(contactPAtient);
        }

        // GET: ContactPAtients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactPAtient contactPAtient = db.ContactPAtients.Find(id);
            if (contactPAtient == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_PATIENT = new SelectList(db.Patients, "ID_Patient", "nom", contactPAtient.ID_PATIENT);
            return View(contactPAtient);
        }

        // POST: ContactPAtients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_ContactPatient,nom,prenom,date_naissance,sexe,profession,situation_familial,groupe_sanguin,email,telephone,ID_PATIENT")] ContactPAtient contactPAtient)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contactPAtient).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID_PATIENT = new SelectList(db.Patients, "ID_Patient", "nom", contactPAtient.ID_PATIENT);
            return View(contactPAtient);
        }

        // GET: ContactPAtients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactPAtient contactPAtient = db.ContactPAtients.Find(id);
            if (contactPAtient == null)
            {
                return HttpNotFound();
            }
            return View(contactPAtient);
        }

        // POST: ContactPAtients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ContactPAtient contactPAtient = db.ContactPAtients.Find(id);
            db.ContactPAtients.Remove(contactPAtient);
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
