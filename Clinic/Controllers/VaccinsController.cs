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
    public class VaccinsController : Controller
    {
        private ClinicEntities db = new ClinicEntities();

        // GET: Vaccins
        public ActionResult Index()
        {
            var vaccins = db.Vaccins.Include(v => v.Consultation).Include(v => v.Patient);
            return View(vaccins.ToList());
        }

        // GET: Vaccins/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vaccin vaccin = db.Vaccins.Find(id);
            if (vaccin == null)
            {
                return HttpNotFound();
            }
            return View(vaccin);
        }

        // GET: Vaccins/Create
        public ActionResult Create()
        {
            ViewBag.ID_Consultation = new SelectList(db.Consultations, "ID_Consultation", "motif");
            ViewBag.ID_PATIENT = new SelectList(db.Patients, "ID_Patient", "nom");
            return View();
        }

        // POST: Vaccins/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_Vaccin,description,date,ID_PATIENT,ID_Consultation")] Vaccin vaccin)
        {
            if (ModelState.IsValid)
            {
                db.Vaccins.Add(vaccin);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ID_Consultation = new SelectList(db.Consultations, "ID_Consultation", "motif", vaccin.ID_Consultation);
            ViewBag.ID_PATIENT = new SelectList(db.Patients, "ID_Patient", "nom", vaccin.ID_PATIENT);
            return View(vaccin);
        }

        // GET: Vaccins/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vaccin vaccin = db.Vaccins.Find(id);
            if (vaccin == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_Consultation = new SelectList(db.Consultations, "ID_Consultation", "motif", vaccin.ID_Consultation);
            ViewBag.ID_PATIENT = new SelectList(db.Patients, "ID_Patient", "nom", vaccin.ID_PATIENT);
            return View(vaccin);
        }

        // POST: Vaccins/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_Vaccin,description,date,ID_PATIENT,ID_Consultation")] Vaccin vaccin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vaccin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID_Consultation = new SelectList(db.Consultations, "ID_Consultation", "motif", vaccin.ID_Consultation);
            ViewBag.ID_PATIENT = new SelectList(db.Patients, "ID_Patient", "nom", vaccin.ID_PATIENT);
            return View(vaccin);
        }

        // GET: Vaccins/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vaccin vaccin = db.Vaccins.Find(id);
            if (vaccin == null)
            {
                return HttpNotFound();
            }
            return View(vaccin);
        }

        // POST: Vaccins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vaccin vaccin = db.Vaccins.Find(id);
            db.Vaccins.Remove(vaccin);
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
