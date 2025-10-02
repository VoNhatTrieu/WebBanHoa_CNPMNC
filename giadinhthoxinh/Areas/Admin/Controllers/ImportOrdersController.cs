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
    public class ImportOrdersController : Controller
    {
        private giadinhthoxinhEntities1 db = new giadinhthoxinhEntities1();

        // GET: Admin/ImportOrders
        public ActionResult Index()
        {
            var tblImportOrders = db.tblImportOrders.Include(t => t.tblUser).Include(t => t.tblSupplier);
            return View(tblImportOrders.ToList());
        }

        // GET: Admin/ImportOrders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblImportOrder tblImportOrder = db.tblImportOrders.Find(id);
            if (tblImportOrder == null)
            {
                return HttpNotFound();
            }
            return View(tblImportOrder);
        }

        // GET: Admin/ImportOrders/Create
        public ActionResult Create()
        {
            ViewBag.FK_iAccountID = new SelectList(db.tblUsers, "PK_iAccountID", "sEmail");
            ViewBag.FK_iSupplierID = new SelectList(db.tblSuppliers, "PK_iSupplierID", "sSupplierName");
            return View();
        }

        // POST: Admin/ImportOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PK_iImportOrderID,FK_iAccountID,FK_iSupplierID,dtDateAdded,sDeliver,iState")] tblImportOrder tblImportOrder)
        {
            if (ModelState.IsValid)
            {
                db.tblImportOrders.Add(tblImportOrder);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FK_iAccountID = new SelectList(db.tblUsers, "PK_iAccountID", "sEmail", tblImportOrder.FK_iAccountID);
            ViewBag.FK_iSupplierID = new SelectList(db.tblSuppliers, "PK_iSupplierID", "sSupplierName", tblImportOrder.FK_iSupplierID);
            return View(tblImportOrder);
        }

        // GET: Admin/ImportOrders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblImportOrder tblImportOrder = db.tblImportOrders.Find(id);
            if (tblImportOrder == null)
            {
                return HttpNotFound();
            }
            ViewBag.FK_iAccountID = new SelectList(db.tblUsers, "PK_iAccountID", "sEmail", tblImportOrder.FK_iAccountID);
            ViewBag.FK_iSupplierID = new SelectList(db.tblSuppliers, "PK_iSupplierID", "sSupplierName", tblImportOrder.FK_iSupplierID);
            return View(tblImportOrder);
        }

        // POST: Admin/ImportOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PK_iImportOrderID,FK_iAccountID,FK_iSupplierID,dtDateAdded,sDeliver,iState")] tblImportOrder tblImportOrder)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblImportOrder).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FK_iAccountID = new SelectList(db.tblUsers, "PK_iAccountID", "sEmail", tblImportOrder.FK_iAccountID);
            ViewBag.FK_iSupplierID = new SelectList(db.tblSuppliers, "PK_iSupplierID", "sSupplierName", tblImportOrder.FK_iSupplierID);
            return View(tblImportOrder);
        }

        // GET: Admin/ImportOrders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblImportOrder tblImportOrder = db.tblImportOrders.Find(id);
            if (tblImportOrder == null)
            {
                return HttpNotFound();
            }
            return View(tblImportOrder);
        }

        // POST: Admin/ImportOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblImportOrder tblImportOrder = db.tblImportOrders.Find(id);
            db.tblImportOrders.Remove(tblImportOrder);
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
