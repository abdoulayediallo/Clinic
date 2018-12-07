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
    public class AdressesController : Controller
    {
        private ClinicEntities db = new ClinicEntities();

        // GET: Adresses
        public ActionResult Index()
        {
            var adresses = db.Adresses.Include(a => a.Patient).Include(a => a.Staff);
            return View(adresses.ToList());
        }

        // GET: Adresses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Adress adress = db.Adresses.Find(id);
            if (adress == null)
            {
                return HttpNotFound();
            }
            return View(adress);
        }

        // GET: Adresses/Create
        public ActionResult Create()
        {
            ViewBag.ID_Patient = new SelectList(db.Patients, "ID_Patient", "nom");
            ViewBag.ID_Staff = new SelectList(db.Staffs, "ID_Staff", "nom");
            ViewBag.ID_ContactPatient = new SelectList(db.ContactPAtients, "ID_ContactPatient", "nom");
            return View();
        }

        // POST: Adresses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_Adresse,pays,ville,prefecture,village,ID_Patient,ID_Staff,ID_ContactPatient")] Adress adress)
        {
            if (ModelState.IsValid)
            {
                db.Adresses.Add(adress);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ID_Patient = new SelectList(db.Patients, "ID_Patient", "nom", adress.ID_Patient);
            ViewBag.ID_Staff = new SelectList(db.Staffs, "ID_Staff", "nom", adress.ID_Staff);
            return View(adress);
        }

        // GET: Adresses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Adress adress = db.Adresses.Find(id);
            if (adress == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_Patient = new SelectList(db.Patients, "ID_Patient", "nom", adress.ID_Patient);
            ViewBag.ID_Staff = new SelectList(db.Staffs, "ID_Staff", "nom", adress.ID_Staff);
            return View(adress);
        }

        // POST: Adresses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_Adresse,pays,ville,prefecture,village,ID_Patient,ID_Staff,ID_ContactPatient")] Adress adress)
        {
            if (ModelState.IsValid)
            {
                db.Entry(adress).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID_Patient = new SelectList(db.Patients, "ID_Patient", "nom", adress.ID_Patient);
            ViewBag.ID_Staff = new SelectList(db.Staffs, "ID_Staff", "nom", adress.ID_Staff);
            return View(adress);
        }

        // GET: Adresses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Adress adress = db.Adresses.Find(id);
            if (adress == null)
            {
                return HttpNotFound();
            }
            return View(adress);
        }

        // POST: Adresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Adress adress = db.Adresses.Find(id);
            db.Adresses.Remove(adress);
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
