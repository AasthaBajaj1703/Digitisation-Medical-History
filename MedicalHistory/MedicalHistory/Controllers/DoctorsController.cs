using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MedicalHistory;

namespace MedicalHistory.Controllers
{
    public class DoctorsController : Controller
    {
        private MedicalHistoryEntities2 db = new MedicalHistoryEntities2();

        // GET: Doctors
        public ActionResult Index()
        {
            return View(db.Doctors.ToList());
        }

        // GET: Doctors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doctor doctor = db.Doctors.Find(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            return View(doctor);
        }

        public ActionResult DoctorLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DoctorLogin(int doctorId, string password)
        {
            Doctor doctor = db.Doctors.Where(p => p.DoctorId == doctorId && p.Password.Equals(password)).FirstOrDefault();
            // int id = patient.Patient_Id;
            if (doctor == null)
            {
                ViewBag.Message = "Wrong Id or Password...";
                return View();

            }
            else
            {
                Session["docid"] = doctor.DoctorId;
                Session["docname"] = doctor.Name;
                return RedirectToAction("FindDetail");
            }
            
        }

        public ActionResult FindDetail()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FindDetail(int patientid)
        {
            //int findid;
            Patient patient = db.Patients.Where(p => p.PatientId == patientid).FirstOrDefault();
            if (patient == null)
            {
                ViewBag.Message = "Patient with Patient Id " + patientid + " does not exist...Please enter the correct Id";
                return View();
            }
            else
            {
                Session["findid"] = patient.PatientId;
                Session["findname"] = patient.Name;
                //findid = Convert.ToInt32(Session["findid"]);
            }

            List<History> history = db.Histories.Where(h => h.PatientId == patientid).ToList();
            if(history.Any())
            {
                
                return RedirectToAction("ViewPatient");
                
            }
            else
            {
                ViewBag.Message = "No history of exist for Patient Id " + patientid;
                return View();
            }
            
        }

        // GET: Doctors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Doctors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DoctorId,Name,Password,City,ContactNo")] Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                db.Doctors.Add(doctor);
                db.SaveChanges();
                ViewBag.Message = "Account Created Successfully...Your ID is " + doctor.DoctorId;
                //return RedirectToAction("Index");
            }

            return View(doctor);
        }

        public ActionResult ViewPatient()
        {
            int findid = Convert.ToInt32(Session["findid"]);
            List<History> history = db.Histories.Where(h => h.PatientId == findid).ToList();
            return View(history);
        }
        
        public ActionResult PatientDocuments(int historyid)
        {
            int patientid = Convert.ToInt32(Session["findid"]);
            List<Document> document = db.Documents.Where(d => d.PatientId == patientid && d.HistoryId == historyid).ToList();
            return View(document);
            
        }
        // GET: Doctors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doctor doctor = db.Doctors.Find(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            return View(doctor);
        }

        // POST: Doctors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DoctorId,Name,Password,City,ContactNo")] Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(doctor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(doctor);
        }

        // GET: Doctors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doctor doctor = db.Doctors.Find(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            return View(doctor);
        }

        // POST: Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Doctor doctor = db.Doctors.Find(id);
            db.Doctors.Remove(doctor);
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
