using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using giadinhthoxinh.Models;

namespace giadinhthoxinh.Areas.Admin.Controllers
{
    public class CheckinDetailsController : Controller
    {
        private giadinhthoxinhEntities1 db = new giadinhthoxinhEntities1();

        // GET: Admin/CheckinDetails
        public ActionResult Index()
        {
            var tblCheckinDetails = db.tblCheckinDetails.Include(t => t.tblImportOrder).Include(t => t.tblProduct);
            return View(tblCheckinDetails.ToList());
        }

        // GET: Admin/CheckinDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblCheckinDetail tblCheckinDetail = db.tblCheckinDetails.Find(id);
            if (tblCheckinDetail == null)
            {
                return HttpNotFound();
            }
            return View(tblCheckinDetail);
        }

        // GET: Admin/CheckinDetails/Create
        public ActionResult Create()
        {
            ViewBag.FK_iImportOrderID = new SelectList(db.tblImportOrders, "PK_iImportOrderID", "sDeliver");
            ViewBag.FK_iProductID = new SelectList(db.tblProducts, "PK_iProductID", "sProductName");
            return View();
        }

        // POST: Admin/CheckinDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PK_iCheckinDetailID,FK_iImportOrderID,FK_iProductID,iQuatity,fPrice")] tblCheckinDetail tblCheckinDetail)
        {
            if (ModelState.IsValid)
            {
                db.tblCheckinDetails.Add(tblCheckinDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FK_iImportOrderID = new SelectList(db.tblImportOrders, "PK_iImportOrderID", "sDeliver", tblCheckinDetail.FK_iImportOrderID);
            ViewBag.FK_iProductID = new SelectList(db.tblProducts, "PK_iProductID", "sProductName", tblCheckinDetail.FK_iProductID);
            return View(tblCheckinDetail);
        }

        // GET: Admin/CheckinDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblCheckinDetail tblCheckinDetail = db.tblCheckinDetails.Find(id);
            if (tblCheckinDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.FK_iImportOrderID = new SelectList(db.tblImportOrders, "PK_iImportOrderID", "sDeliver", tblCheckinDetail.FK_iImportOrderID);
            ViewBag.FK_iProductID = new SelectList(db.tblProducts, "PK_iProductID", "sProductName", tblCheckinDetail.FK_iProductID);
            return View(tblCheckinDetail);
        }

        // POST: Admin/CheckinDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PK_iCheckinDetailID,FK_iImportOrderID,FK_iProductID,iQuatity,fPrice")] tblCheckinDetail tblCheckinDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblCheckinDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FK_iImportOrderID = new SelectList(db.tblImportOrders, "PK_iImportOrderID", "sDeliver", tblCheckinDetail.FK_iImportOrderID);
            ViewBag.FK_iProductID = new SelectList(db.tblProducts, "PK_iProductID", "sProductName", tblCheckinDetail.FK_iProductID);
            return View(tblCheckinDetail);
        }

        // GET: Admin/CheckinDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblCheckinDetail tblCheckinDetail = db.tblCheckinDetails.Find(id);
            if (tblCheckinDetail == null)
            {
                return HttpNotFound();
            }
            return View(tblCheckinDetail);
        }

        // POST: Admin/CheckinDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblCheckinDetail tblCheckinDetail = db.tblCheckinDetails.Find(id);
            db.tblCheckinDetails.Remove(tblCheckinDetail);
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
