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
    public class PatientsController : Controller
    {
        private ClinicEntities db = new ClinicEntities();

        // GET: Patients
        public ActionResult Index()
        {
            return View(db.Patients.ToList());
        }

        // GET: Patients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // GET: Patients/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Patients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        public ContactPAtient GetContactPAtient(int idContactPatient)
        {
            ContactPAtient contactP = new ContactPAtient();
            using (var cp = new ClinicEntities())
            {
                var obj = cp.ContactPAtients
                                            .Where(contactPatient => contactPatient.ID_ContactPatient == idContactPatient)
                                            .Select(st => new {
                                                Nom = st.nom,
                                                Prenom = st.prenom,
                                                DateNaissance = st.date_naissance,
                                                Sexe = st.sexe,
                                                Profession = st.profession,
                                                SituationF = st.situation_familial,
                                                GroupeSanguin = st.groupe_sanguin,
                                                Mail = st.email,
                                                Phone = st.telephone,
                                                IdCp = st.ID_ContactPatient
                                            });

                contactP.nom = obj.Select(x => x.Nom).DefaultIfEmpty("").First();
                contactP.prenom = obj.Select(x => x.Prenom).DefaultIfEmpty("").First();
                contactP.date_naissance = obj.Select(x => x.DateNaissance).DefaultIfEmpty().First();
                contactP.sexe = obj.Select(x => x.Sexe).DefaultIfEmpty("").First();
                contactP.profession = obj.Select(x => x.Profession).DefaultIfEmpty("").First();
                contactP.situation_familial = obj.Select(x => x.SituationF).DefaultIfEmpty("").First();
                contactP.groupe_sanguin = obj.Select(x => x.GroupeSanguin).DefaultIfEmpty("").First();
                contactP.email = obj.Select(x => x.Mail).DefaultIfEmpty("").First();
                contactP.telephone = obj.Select(x => x.Phone).DefaultIfEmpty("").DefaultIfEmpty("").First();
                contactP.ID_ContactPatient = obj.Select(x => x.IdCp).DefaultIfEmpty(0).First();
            }
            return contactP;
        }

        public Adress GetAdress(int idPatient)
        {
            Adress adr = new Adress();
            using (var cp = new ClinicEntities())
            {
                var obj = cp.Adresses
                                            .Where(adresse => adresse.ID_Patient == idPatient)
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_Patient,nom,prenom,date_naissance,sexe,profession,situation_familial,groupe_sanguin,email,telephone,dateCreation")] Patient patient)
        {
            Adress adress = new Adress();
            ContactPAtient cp = new ContactPAtient();
            if (ModelState.IsValid)
            {
                patient.dateCreation = DateTime.Now;
                db.Patients.Add(patient);
                string pays = Request["pays"].ToString();
                string ville = Request["ville"].ToString();
                string prefecture = Request["prefecture"].ToString();
                string village = Request["village"].ToString();
                if (!string.IsNullOrEmpty(pays) || !string.IsNullOrEmpty(ville) || !string.IsNullOrEmpty(prefecture) || !string.IsNullOrEmpty(village))
                {
                    adress.ID_Patient = patient.ID_Patient;
                    adress.pays = !string.IsNullOrEmpty(pays) ? pays : "";
                    adress.ville = !string.IsNullOrEmpty(ville) ? ville : "";
                    adress.prefecture = !string.IsNullOrEmpty(prefecture) ? prefecture : "";
                    adress.village = !string.IsNullOrEmpty(village) ? village : "";
                    db.Adresses.Add(adress);
                }
                    db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(patient);
        }

        // GET: Patients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }

            patient.adress = GetAdress(patient.ID_Patient);

            patient.cp = GetContactPAtient(patient.ID_Patient);
            return View(patient);
        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_Patient,nom,prenom,date_naissance,sexe,profession,situation_familial,groupe_sanguin,email,telephone")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                db.Entry(patient).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(patient);
        }

        // GET: Patients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Patient patient = db.Patients.Find(id);
            db.Patients.Remove(patient);
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
