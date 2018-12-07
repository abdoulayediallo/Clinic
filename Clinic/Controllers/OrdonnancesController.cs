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
    public class OrdonnancesController : Controller
    {
        private ClinicEntities db = new ClinicEntities();

        // GET: Ordonnances
        public ActionResult Index()
        {
            var ordonnances = db.Ordonnances.Include(o => o.Consultation);
            return View(ordonnances.ToList());
        }

        // GET: Ordonnances/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ordonnance ordonnance = db.Ordonnances.Find(id);
            if (ordonnance == null)
            {
                return HttpNotFound();
            }
            return View(ordonnance);
        }

        // GET: Ordonnances/Create
        public ActionResult Create()
        {
            ViewBag.ID_Consultation = new SelectList(db.Consultations, "ID_Consultation", "motif");
            return View();
        }

        // POST: Ordonnances/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_Ordonnance,prescription,medicament,date,ID_Consultation")] Ordonnance ordonnance)
        {
            if (ModelState.IsValid)
            {
                db.Ordonnances.Add(ordonnance);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ID_Consultation = new SelectList(db.Consultations, "ID_Consultation", "motif", ordonnance.ID_Consultation);
            return View(ordonnance);
        }

        // GET: Ordonnances/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ordonnance ordonnance = db.Ordonnances.Find(id);
            if (ordonnance == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_Consultation = new SelectList(db.Consultations, "ID_Consultation", "motif", ordonnance.ID_Consultation);
            return View(ordonnance);
        }

        // POST: Ordonnances/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_Ordonnance,prescription,medicament,date,ID_Consultation")] Ordonnance ordonnance)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ordonnance).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID_Consultation = new SelectList(db.Consultations, "ID_Consultation", "motif", ordonnance.ID_Consultation);
            return View(ordonnance);
        }

        // GET: Ordonnances/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ordonnance ordonnance = db.Ordonnances.Find(id);
            if (ordonnance == null)
            {
                return HttpNotFound();
            }
            return View(ordonnance);
        }

        // POST: Ordonnances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ordonnance ordonnance = db.Ordonnances.Find(id);
            db.Ordonnances.Remove(ordonnance);
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
