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
    public class AntecedentsController : Controller
    {
        private ClinicEntities db = new ClinicEntities();

        // GET: Antecedents
        public ActionResult Index()
        {
            var antecedents = db.Antecedents.Include(a => a.Consultation);
            return View(antecedents.ToList());
        }

        // GET: Antecedents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Antecedent antecedent = db.Antecedents.Find(id);
            if (antecedent == null)
            {
                return HttpNotFound();
            }
            return View(antecedent);
        }

        // GET: Antecedents/Create
        public ActionResult Create()
        {
            ViewBag.ID_Consultation = new SelectList(db.Consultations, "ID_Consultation", "motif");
            return View();
        }

        // POST: Antecedents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_Antecedent,description,ID_Consultation")] Antecedent antecedent)
        {
            if (ModelState.IsValid)
            {
                db.Antecedents.Add(antecedent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ID_Consultation = new SelectList(db.Consultations, "ID_Consultation", "motif", antecedent.ID_Consultation);
            return View(antecedent);
        }

        // GET: Antecedents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Antecedent antecedent = db.Antecedents.Find(id);
            if (antecedent == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_Consultation = new SelectList(db.Consultations, "ID_Consultation", "motif", antecedent.ID_Consultation);
            return View(antecedent);
        }

        // POST: Antecedents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_Antecedent,description,ID_Consultation")] Antecedent antecedent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(antecedent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID_Consultation = new SelectList(db.Consultations, "ID_Consultation", "motif", antecedent.ID_Consultation);
            return View(antecedent);
        }

        // GET: Antecedents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Antecedent antecedent = db.Antecedents.Find(id);
            if (antecedent == null)
            {
                return HttpNotFound();
            }
            return View(antecedent);
        }

        // POST: Antecedents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Antecedent antecedent = db.Antecedents.Find(id);
            db.Antecedents.Remove(antecedent);
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
