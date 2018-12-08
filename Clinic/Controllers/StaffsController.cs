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
    public class StaffsController : Controller
    {
        private ClinicEntities db = new ClinicEntities();

        // GET: Staffs
        public ActionResult Index()
        {
            return View(db.Staffs.ToList());
        }

        // GET: Staffs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff staff = db.Staffs.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            staff.adress = GetAdress(staff.ID_Staff);
            return View(staff);
        }
        public Adress GetAdress(int idStaff)
        {
            Adress adr = new Adress();
            using (var cp = new ClinicEntities())
            {
                var obj = cp.Adresses
                                            .Where(adresse => adresse.ID_Staff == idStaff)
                                            .Select(st => new {
                                                Pays = st.pays,
                                                Ville = st.ville,
                                                Prefecture = st.prefecture,
                                                Village = st.village
                                            });

                adr.pays = obj.Select(x => x.Pays).DefaultIfEmpty("").First();
                adr.ville = obj.Select(x => x.Ville).DefaultIfEmpty("").First();
                adr.prefecture = obj.Select(x => x.Prefecture).DefaultIfEmpty("").First();
                adr.village = obj.Select(x => x.Village).DefaultIfEmpty("").First();
            }
            return adr;
        }
        // GET: Staffs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Staffs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_Staff,nom,prenom,sexe,phone,email,login,password,role")] Staff staff)
        {
            Adress adress = new Adress();
            if (ModelState.IsValid)
            {
                db.Staffs.Add(staff);

                string pays = Request["pays"].ToString();
                string ville = Request["ville"].ToString();
                string prefecture = Request["prefecture"].ToString();
                string village = Request["village"].ToString();
                if (!string.IsNullOrEmpty(pays) || !string.IsNullOrEmpty(ville) || !string.IsNullOrEmpty(prefecture) || !string.IsNullOrEmpty(village))
                {
                    adress.ID_Staff = staff.ID_Staff;
                    adress.pays = !string.IsNullOrEmpty(pays) ? pays : "";
                    adress.ville = !string.IsNullOrEmpty(ville) ? ville : "";
                    adress.prefecture = !string.IsNullOrEmpty(prefecture) ? prefecture : "";
                    adress.village = !string.IsNullOrEmpty(village) ? village : "";
                    db.Adresses.Add(adress);
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(staff);
        }

        // GET: Staffs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff staff = db.Staffs.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            staff.adress = GetAdress(staff.ID_Staff);
            return View(staff);
        }

        // POST: Staffs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_Staff,nom,prenom,sexe,phone,email,login,password,role")] Staff staff)
        {
            if (ModelState.IsValid)
            {
                db.Entry(staff).State = EntityState.Modified;

                Adress adrc = new Adress();

                string pays = Request["adress.pays"].ToString();
                string ville = Request["adress.ville"].ToString();
                string prefecture = Request["adress.prefecture"].ToString();
                string village = Request["adress.village"].ToString();

                int idAdrc = db.Adresses.Where(id => id.ID_Staff == staff.ID_Staff).Select(x => x.ID_Adresse).DefaultIfEmpty(0).First();

                if (idAdrc > 0)
                {
                    if (!string.IsNullOrEmpty(pays))
                    {
                        db.Database.ExecuteSqlCommand("Update Adresses set pays='" + pays.ToString() + "' where ID_Adresse =" + idAdrc);
                    }
                    if (!string.IsNullOrEmpty(ville))
                    {
                        db.Database.ExecuteSqlCommand("Update Adresses set ville='" + ville.ToString() + "' where ID_Adresse =" + idAdrc);
                    }
                    if (!string.IsNullOrEmpty(prefecture))
                    {
                        db.Database.ExecuteSqlCommand("Update Adresses set prefecture='" + prefecture.ToString() + "' where ID_Adresse =" + idAdrc);
                    }
                    if (!string.IsNullOrEmpty(village))
                    {
                        db.Database.ExecuteSqlCommand("Update Adresses set village='" + village.ToString() + "' where ID_Adresse =" + idAdrc);
                    }
                }
                if (idAdrc == 0)
                {
                    if (!string.IsNullOrEmpty(pays) || !string.IsNullOrEmpty(ville) || !string.IsNullOrEmpty(prefecture) || !string.IsNullOrEmpty(village))
                    {
                        adrc.ID_Staff = staff.ID_Staff;
                        adrc.pays = pays;
                        adrc.ville = ville;
                        adrc.prefecture = prefecture;
                        adrc.village = village;
                        db.Adresses.Add(adrc);
                    }

                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(staff);
        }

        // GET: Staffs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff staff = db.Staffs.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            return View(staff);
        }

        // POST: Staffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Staff staff = db.Staffs.Find(id);
            db.Staffs.Remove(staff);
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
