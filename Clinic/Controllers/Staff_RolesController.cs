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
    public class Staff_RolesController : Controller
    {
        private ClinicEntities db = new ClinicEntities();

        // GET: Staff_Roles
        public ActionResult Index()
        {
            var staff_Roles = db.Staff_Roles.Include(s => s.Role).Include(s => s.Staff);
            return View(staff_Roles.ToList());
        }

        // GET: Staff_Roles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff_Roles staff_Roles = db.Staff_Roles.Find(id);
            if (staff_Roles == null)
            {
                return HttpNotFound();
            }
            return View(staff_Roles);
        }

        // GET: Staff_Roles/Create
        public ActionResult Create()
        {
            ViewBag.ID_Role = new SelectList(db.Roles, "ID_Role", "description");
            ViewBag.ID_Staff = new SelectList(db.Staffs, "ID_Staff", "nom");
            return View();
        }

        // POST: Staff_Roles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_Staff_Role,ID_Staff,ID_Role")] Staff_Roles staff_Roles)
        {
            if (ModelState.IsValid)
            {
                db.Staff_Roles.Add(staff_Roles);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ID_Role = new SelectList(db.Roles, "ID_Role", "description", staff_Roles.ID_Role);
            ViewBag.ID_Staff = new SelectList(db.Staffs, "ID_Staff", "nom", staff_Roles.ID_Staff);
            return View(staff_Roles);
        }

        // GET: Staff_Roles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff_Roles staff_Roles = db.Staff_Roles.Find(id);
            if (staff_Roles == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_Role = new SelectList(db.Roles, "ID_Role", "description", staff_Roles.ID_Role);
            ViewBag.ID_Staff = new SelectList(db.Staffs, "ID_Staff", "nom", staff_Roles.ID_Staff);
            return View(staff_Roles);
        }

        // POST: Staff_Roles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_Staff_Role,ID_Staff,ID_Role")] Staff_Roles staff_Roles)
        {
            if (ModelState.IsValid)
            {
                db.Entry(staff_Roles).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID_Role = new SelectList(db.Roles, "ID_Role", "description", staff_Roles.ID_Role);
            ViewBag.ID_Staff = new SelectList(db.Staffs, "ID_Staff", "nom", staff_Roles.ID_Staff);
            return View(staff_Roles);
        }

        // GET: Staff_Roles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff_Roles staff_Roles = db.Staff_Roles.Find(id);
            if (staff_Roles == null)
            {
                return HttpNotFound();
            }
            return View(staff_Roles);
        }

        // POST: Staff_Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Staff_Roles staff_Roles = db.Staff_Roles.Find(id);
            db.Staff_Roles.Remove(staff_Roles);
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
