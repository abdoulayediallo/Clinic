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
            patient.adress = GetAdress(patient.ID_Patient);
            patient.cp = GetContactPAtient(patient.ID_Patient);
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
                                            .Where(contactPatient => contactPatient.ID_PATIENT == idContactPatient)
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
                Adress adrc = new Adress();
                ContactPAtient cp = new ContactPAtient();

                string pays = Request["adress.pays"].ToString();
                string ville = Request["adress.ville"].ToString();
                string prefecture = Request["adress.prefecture"].ToString();
                string village = Request["adress.village"].ToString();

                string nomCp = Request["cp.nom"].ToString();
                string prenomCp = Request["cp.prenom"].ToString();
                string sexeCp = Request["cp.sexe"].ToString();
                string professionCp = Request["cp.profession"].ToString();
                string emailCp = Request["cp.email"].ToString();
                string telephoneCp = Request["cp.telephone"].ToString();

                int idAdrc = db.Adresses.Where(id => id.ID_Patient == patient.ID_Patient).Select(x => x.ID_Adresse).DefaultIfEmpty(0).First();
                int idCp = db.ContactPAtients.Where(id => id.ID_PATIENT == patient.ID_Patient).Select(x => x.ID_ContactPatient).DefaultIfEmpty(0).First();

                if(idAdrc > 0)
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
                        adrc.ID_Patient = patient.ID_Patient;
                        adrc.pays = pays;
                        adrc.ville = ville;
                        adrc.prefecture = prefecture;
                        adrc.village = village;
                        db.Adresses.Add(adrc);
                    }
                   
                }

                if(idCp > 0)
                {
                    if (!string.IsNullOrEmpty(nomCp))
                    {
                        db.Database.ExecuteSqlCommand("Update ContactPAtients set nom='" + nomCp.ToString() + "' where ID_ContactPatient =" + idCp);
                    }
                    if (!string.IsNullOrEmpty(prenomCp))
                    {
                        db.Database.ExecuteSqlCommand("Update ContactPAtients set prenom='" + prenomCp.ToString() + "' where ID_ContactPatient =" + idCp);
                    }
                    if (!string.IsNullOrEmpty(sexeCp))
                    {
                        db.Database.ExecuteSqlCommand("Update ContactPAtients set sexe='" + sexeCp.ToString() + "' where ID_ContactPatient =" + idCp);
                    }
                    if (!string.IsNullOrEmpty(professionCp))
                    {
                        db.Database.ExecuteSqlCommand("Update ContactPAtients set profession='" + professionCp.ToString() + "' where ID_ContactPatient =" + idCp);
                    }
                    if (!string.IsNullOrEmpty(emailCp))
                    {
                        db.Database.ExecuteSqlCommand("Update ContactPAtients set email='" + emailCp.ToString() + "' where ID_ContactPatient =" + idCp);
                    }
                    if (!string.IsNullOrEmpty(telephoneCp))
                    {
                        db.Database.ExecuteSqlCommand("Update ContactPAtients set telephone='" + telephoneCp.ToString() + "' where ID_ContactPatient =" + idCp);
                    }
                }
                if(idCp == 0)
                {
                    if(!string.IsNullOrEmpty(nomCp) || !string.IsNullOrEmpty(prenomCp) || !string.IsNullOrEmpty(sexeCp) || !string.IsNullOrEmpty(professionCp) || !string.IsNullOrEmpty(emailCp) || !string.IsNullOrEmpty(telephoneCp))
                    {
                        cp.ID_PATIENT = patient.ID_Patient;
                        cp.nom = nomCp;
                        cp.prenom = prenomCp;
                        cp.sexe = sexeCp;
                        cp.profession = professionCp;
                        cp.email = emailCp;
                        cp.telephone = telephoneCp;
                        db.ContactPAtients.Add(cp);
                    }
                    
                }
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
