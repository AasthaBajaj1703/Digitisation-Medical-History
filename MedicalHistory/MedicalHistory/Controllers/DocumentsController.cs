using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MedicalHistory;

namespace MedicalHistory.Controllers
{
    public class DocumentsController : Controller
    {
        private MedicalHistoryEntities2 db = new MedicalHistoryEntities2();

        // GET: Documents
        public ActionResult Index()
        {
            var documents = db.Documents.Include(d => d.History).Include(d => d.Patient);
            return View(documents.ToList());
        }

        // GET: Documents/Details/5
        public ActionResult Details(int? historyid)
        {
            if (historyid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Session["hid"] = historyid;
            int patientid = Convert.ToInt32(Session["id"]);
            //Document document = db.Documents.Find(historyid);
            //if (document == null)
            //{
            //    return HttpNotFound();
            //}
            List<Document> document = db.Documents.Where(d => d.PatientId == patientid && d.HistoryId == historyid).ToList();
            return View(document);
        }

        //[HttpGet]
        //public ActionResult View()
        //{

        //}

        //[HttpPost]
        //public ActionResult View()
        //{

        //}

        [HttpGet]
        public ActionResult Add(int historyid)
        {
            Session["hid"] = historyid;
            Document document = new Document();
            document.PatientId = Convert.ToInt32(Session["id"]);
            document.HistoryId = Convert.ToInt32(Session["hid"]);
            return View(document);
        }

        [HttpPost]
        public ActionResult Add(Document document)
        {
            string fileName = Path.GetFileNameWithoutExtension(document.ImageFile.FileName);
            string extension = Path.GetExtension(document.ImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            document.Path = "~/PatientDocument/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/PatientDocument/"), fileName);
            document.ImageFile.SaveAs(fileName);
            using (MedicalHistoryEntities2 db = new MedicalHistoryEntities2())
            {
                db.Documents.Add(document);
                db.SaveChanges();
            }
            ViewBag.Message = "Document Uploaded Successfully....";
            //ModelState.Clear();
            return View();
        }

        //[HttpGet]
        // GET: Documents/Create
        //public ActionResult Create(int id)
        //{
        //    Session["hid"] = id;
        //    Document document = new Document();
        //    document.PatientId = Convert.ToInt32(Session["id"]);
        //    document.HistoryId = Convert.ToInt32(Session["hid"]);
        //    //ViewBag.HistoryId = new SelectList(db.Histories, "HistoryId", "Disease");
        //    //ViewBag.PatientId = new SelectList(db.Patients, "PatientId", "Name");
        //    return View(document);
        //}

        //// POST: Documents/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        ////[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "PatientId,HistoryId,Path")] Document document)
        //{
        //    string fileName = Path.GetFileNameWithoutExtension(document.ImageFile.FileName);
        //    string extension = Path.GetExtension(document.ImageFile.FileName);
        //    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
        //    document.Path = "~/PatientDocument/" + fileName;
        //    fileName = Path.Combine(Server.MapPath("~/PatientDocument/"), fileName);
        //    document.ImageFile.SaveAs(fileName);
        //    using (MedicalHistoryEntities2 db = new MedicalHistoryEntities2())
        //    {
        //        db.Documents.Add(document);
        //        db.SaveChanges();
        //    }
        //    ModelState.Clear();
        //    return View();
        //}

        // GET: Documents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            ViewBag.HistoryId = new SelectList(db.Histories, "HistoryId", "Disease", document.HistoryId);
            ViewBag.PatientId = new SelectList(db.Patients, "PatientId", "Name", document.PatientId);
            return View(document);
        }

        // POST: Documents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PatientId,HistoryId,Path")] Document document)
        {
            if (ModelState.IsValid)
            {
                db.Entry(document).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HistoryId = new SelectList(db.Histories, "HistoryId", "Disease", document.HistoryId);
            ViewBag.PatientId = new SelectList(db.Patients, "PatientId", "Name", document.PatientId);
            return View(document);
        }

        // GET: Documents/Delete/5
        public ActionResult Delete(int? documentid)
        {
            if (documentid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = db.Documents.Find(documentid);
            if (document == null)
            {
                return HttpNotFound();
            }
            return View(document);
        }

        // POST: Documents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int documentid)
        {
            Document document = db.Documents.Find(documentid);
            db.Documents.Remove(document);
            db.SaveChanges();
            return RedirectToAction("Details",new { historyid = Session["hid"] });
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
