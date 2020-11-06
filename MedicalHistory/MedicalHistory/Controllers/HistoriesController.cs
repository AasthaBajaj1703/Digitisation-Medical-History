using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MedicalHistory;

namespace MedicalHistory.Controllers
{
    public class HistoriesController : Controller
    {
        private MedicalHistoryEntities2 db = new MedicalHistoryEntities2();

        // GET: Histories
        public ActionResult Index()
        {
            var histories = db.Histories.Include(h => h.Patient);
            return View(histories.ToList());
        }

        // GET: Histories/Details/5
        public ActionResult Details(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //History history = db.Histories.Find(id);
            List<History> history = db.Histories.Where(h => h.PatientId == id).ToList();
            
            if (history == null)
            {
                return RedirectToAction("Create");
            }
            return View(history);
        }

        // GET: Histories/Create
        public ActionResult Create()
        {
            //ViewBag.PatientId = new SelectList(db.Patients, "PatientId", "Name");
            History history = new History();
            history.PatientId = Convert.ToInt32(Session["id"]);
            //Selectlist(from where we want to select list, what to store in variable, what to pick from 
            //list and show in dropdown)
            return View(history);
        }

        // POST: Histories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "HistoryId,PatientId,Disease,Hospital,City,Doctor,DateFrom,DateTo")] History history)
        {
            if (ModelState.IsValid)
            {
                db.Histories.Add(history);
                db.SaveChanges();
                return RedirectToAction("Details",new { id = Session["id"] });
            }

            //ViewBag.PatientId = new SelectList(db.Patients, "PatientId", "Name", history.PatientId);
            return View(history);
        }

        // GET: Histories/Edit/5
        public ActionResult Edit(int? historyid)
        {
            if (historyid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            History history = db.Histories.Find(historyid);
            if (history == null)
            {
                return HttpNotFound();
            }
            Session["hid"] = history.HistoryId;
            history.PatientId = Convert.ToInt32(Session["id"]);
            return View(history);
        }

        // POST: Histories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "HistoryId,PatientId,Disease,Hospital,City,Doctor,DateFrom,DateTo")] History history)
        {
            if (ModelState.IsValid)
            {
                db.Entry(history).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details",new { id = Session["id"] });
            }
            ViewBag.PatientId = new SelectList(db.Patients, "PatientId", "Name", history.PatientId);
            return View(history);
        }

        // GET: Histories/Delete/5
        public ActionResult Delete(int? historyid)
        {
            if (historyid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            History history = db.Histories.Find(historyid);
            if (history == null)
            {
                return HttpNotFound();
            }
            Session["hid"] = history.HistoryId;
            return View(history);
        }

        // POST: Histories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int historyid)
        {
            History history = db.Histories.Find(historyid);
            db.Histories.Remove(history);
            db.SaveChanges();
            return RedirectToAction("Details",new { id = Session["id"] });
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
