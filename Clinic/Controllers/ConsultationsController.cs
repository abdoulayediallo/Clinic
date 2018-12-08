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
    public class ConsultationsController : Controller
    {
        private ClinicEntities db = new ClinicEntities();

        // GET: Consultations
        public ActionResult Index()
        {
            var consultations = db.Consultations.Include(c => c.Patient).Include(c => c.Staff);
            return View(consultations.ToList());
        }

        // GET: Consultations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Consultation consultation = db.Consultations.Find(id);
            if (consultation == null)
            {
                return HttpNotFound();
            }
            consultation.vaccin.description = getVaccin(consultation.ID_Consultation);
            consultation.ordonnance.prescription = getPrecription(consultation.ID_Consultation);
            consultation.ordonnance.medicament = getMedicament(consultation.ID_Consultation);
            consultation.antecedent.description = getAntecedent(consultation.ID_Consultation);
            return View(consultation);
        }

        // GET: Consultations/Create
        public ActionResult Create()
        {
            ViewBag.ID_Patient = new SelectList(db.Patients, "ID_Patient", "nom");
            ViewBag.ID_Staff = new SelectList(db.Staffs, "ID_Staff", "nom");
            return View();
        }

        // POST: Consultations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_Consultation,dateCreation,motif,note,poids,taille,temperature,systol,diastol,diagnostique,maladie,ID_Patient,ID_Staff")] Consultation consultation)
        {
            Vaccin vaccin = new Vaccin();
            Ordonnance ordonnance = new Ordonnance();
            Antecedent antecedentConsultation = new Antecedent();
            if (ModelState.IsValid)
            {
                db.Consultations.Add(consultation);
                string prescription = Request["prescription"].ToString();
                string medicament = Request["medicament"].ToString();
                string vac = Request["vac"].ToString();
                string antecedent = Request["ant"];

                if (!string.IsNullOrEmpty(antecedent))
                {
                    antecedentConsultation.ID_Consultation = consultation.ID_Consultation;
                    antecedentConsultation.ID_Patient = consultation.ID_Patient;
                    antecedentConsultation.description = antecedent;
                    db.Antecedents.Add(antecedentConsultation);
                }

                if (!string.IsNullOrEmpty(vac))
                {
                    vaccin.ID_Consultation = consultation.ID_Consultation;
                    vaccin.ID_PATIENT = consultation.ID_Patient;
                    vaccin.description = vac;
                    vaccin.date = DateTime.Now;
                    db.Vaccins.Add(vaccin);
                }
                if (!string.IsNullOrEmpty(medicament))
                {
                    ordonnance.ID_Consultation = consultation.ID_Consultation;
                    ordonnance.medicament = medicament;
                    ordonnance.prescription = !string.IsNullOrEmpty(prescription) ? prescription : "";
                    db.Ordonnances.Add(ordonnance);
                }
                
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID_Patient = new SelectList(db.Patients, "ID_Patient", "nom", consultation.ID_Patient);
            ViewBag.ID_Staff = new SelectList(db.Staffs, "ID_Staff", "nom", consultation.ID_Staff);
            
            return View(consultation);
        }

        // GET: Consultations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Consultation consultation = db.Consultations.Find(id);
            if (consultation == null)
            {
                return HttpNotFound();
            }
            consultation.antecedent.description = getAntecedent(consultation.ID_Consultation);
            consultation.vaccin.description = getVaccin(consultation.ID_Consultation);
            consultation.ordonnance.prescription = getPrecription(consultation.ID_Consultation);
            consultation.ordonnance.medicament = getMedicament(consultation.ID_Consultation);
            ViewBag.ID_Patient = new SelectList(db.Patients, "ID_Patient", "nom", consultation.ID_Patient);
            ViewBag.ID_Staff = new SelectList(db.Staffs, "ID_Staff", "nom", consultation.ID_Staff);
            return View(consultation);
        }
        public string getVaccin(int idConsultation)
        {
            string descriptionVaccin = db.Vaccins.Where(vaccin => vaccin.ID_Consultation == idConsultation).Select(x => x.description).DefaultIfEmpty("").First();
            return descriptionVaccin;
        }

        public string getPrecription(int idConsultation)
        {
            string prescription = db.Ordonnances.Where(ordonnance => ordonnance.ID_Consultation == idConsultation).Select(x => x.prescription).DefaultIfEmpty("").First();
            return prescription;
        }

        public string getMedicament(int idConsultation)
        {
            string medicament = db.Ordonnances.Where(ordonnance => ordonnance.ID_Consultation == idConsultation).Select(x => x.medicament).DefaultIfEmpty("").First();
            return medicament;
        }

        public string getAntecedent(int idConsultation)
        {
            string antecedent = db.Antecedents.Where(a => a.ID_Consultation == idConsultation).Select(x => x.description).DefaultIfEmpty("").First();
            return antecedent;
        }
        // POST: Consultations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_Consultation,dateCreation,motif,note,poids,taille,temperature,systol,diastol,diagnostique,maladie,ID_Patient,ID_Staff")] Consultation consultation
            /*[Bind(Include = "id_audit,ID_Consultation,ID_Staff,change_date,change_by")] Audit_Consultation ac*/)
        {

            //string medicament = (from o in db.Ordonnances where o.ID_Consultation == consultation.ID_Consultation select new { o.medicament }).Single().medicament;
            //string prescription = (from o in db.Ordonnances where o.ID_Consultation == consultation.ID_Consultation select new { o.prescription }).Single().prescription;
            //string vaccin = (from v in db.Vaccins where v.ID_Consultation == consultation.ID_Consultation select new { v.description }).Single().description;
            //consultation.vaccin.description = getVaccin(consultation.ID_Consultation); 
            //consultation.ordonnance.prescription = getPrecription(consultation.ID_Consultation);
            //consultation.ordonnance.medicament = getMedicament(consultation.ID_Consultation);
            if (ModelState.IsValid)
            {
                db.Entry(consultation).State = EntityState.Modified;
                //ac.ID_Consultation = consultation.ID_Consultation;
                //ac.ID_Staff = consultation.ID_Staff;
                //ac.change_date = DateTime.Now;
                Vaccin v = new Vaccin();
                Ordonnance o = new Ordonnance();
                Antecedent ant = new Antecedent();
                var changeBy = (from s in db.Staffs where s.ID_Staff == consultation.ID_Staff select new {s.nom, s.prenom}).Single();
                consultation.changeBy= changeBy.nom +" "+ changeBy.prenom;
                consultation.changeDate = DateTime.Now;
                //db.Audit_Consultation.Add(ac);
                string viewAntecedent = Request["antecedent.description"].ToString();
                string viewVaccin = Request["vaccin.description"].ToString();
                string viewPrescription = Request["ordonnance.prescription"].ToString();
                string viewMedicament = Request["ordonnance.medicament"].ToString();

                int idVac = db.Vaccins.Where(id => id.ID_Consultation == consultation.ID_Consultation).Select(x => x.ID_Vaccin).DefaultIfEmpty(0).First();
                int idOrd = db.Ordonnances.Where(id => id.ID_Consultation == consultation.ID_Consultation).Select(x => x.ID_Ordonnance).DefaultIfEmpty(0).First();
                int idAnt = db.Antecedents.Where(id => id.ID_Consultation == consultation.ID_Consultation).Select(x => x.ID_Antecedent).DefaultIfEmpty(0).First();

                if (idAnt > 0)
                {
                    if (!string.IsNullOrEmpty(viewAntecedent))
                    {
                        db.Database.ExecuteSqlCommand("Update Antecedents set description='" + viewAntecedent + "' where ID_Antecedent =" + idAnt);
                    }
                }
                if (idAnt == 0)
                {
                    if (!string.IsNullOrEmpty(viewAntecedent))
                    {
                        ant.description = viewAntecedent.ToString();
                        ant.ID_Consultation = consultation.ID_Consultation;
                        ant.ID_Patient= consultation.ID_Patient;
                        db.Antecedents.Add(ant);
                    }

                }

                if (idVac > 0)
                {
                    if (!string.IsNullOrEmpty(viewVaccin))
                    {
                        db.Database.ExecuteSqlCommand("Update Vaccins set description='" + viewVaccin.ToString() + "' where ID_Vaccin =" + idVac);
                    }
                }
                if (idVac == 0)
                {
                    if (!string.IsNullOrEmpty(viewVaccin))
                    {
                        v.description = viewVaccin.ToString();
                        v.ID_Consultation = consultation.ID_Consultation;
                        v.ID_PATIENT = consultation.ID_Patient;
                        v.date = DateTime.Now;
                        db.Vaccins.Add(v);
                    }
                   
                }
                if (idOrd > 0)
                {
                    if (!string.IsNullOrEmpty(viewPrescription))
                    {
                        db.Database.ExecuteSqlCommand("Update Ordonnances set prescription='" + viewPrescription.ToString() + "' where ID_Ordonnance =" + idOrd);
                    }
                    //else
                    //{
                    //    db.Database.ExecuteSqlCommand("Update Ordonnances set prescription='' where ID_Ordonnance =" + idOrd);
                    //}
                    if (!string.IsNullOrEmpty(viewMedicament))
                    {
                        db.Database.ExecuteSqlCommand("Update Ordonnances set medicament='" + viewMedicament.ToString() + "' where ID_Ordonnance =" + idOrd);
                    }
                   
                }
                if (idOrd == 0)
                {
                    if (!string.IsNullOrEmpty(viewPrescription))
                    {
                        o.prescription = viewPrescription.ToString();
                        o.ID_Consultation = consultation.ID_Consultation;
                        db.Ordonnances.Add(o);
                    }
                    if (!string.IsNullOrEmpty(viewMedicament))
                    {
                        o.medicament = viewMedicament.ToString();
                        o.ID_Consultation = consultation.ID_Consultation;
                        db.Ordonnances.Add(o);
                    }
                    
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            ViewBag.ID_Patient = new SelectList(db.Patients, "ID_Patient", "nom", consultation.ID_Patient);
            ViewBag.ID_Staff = new SelectList(db.Staffs, "ID_Staff", "nom", consultation.ID_Staff);

            
            return View(consultation);
        }

     
        // GET: Consultations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Consultation consultation = db.Consultations.Find(id);
            if (consultation == null)
            {
                return HttpNotFound();
            }
            return View(consultation);
        }

        // POST: Consultations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Consultation consultation = db.Consultations.Find(id);
            db.Consultations.Remove(consultation);
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
