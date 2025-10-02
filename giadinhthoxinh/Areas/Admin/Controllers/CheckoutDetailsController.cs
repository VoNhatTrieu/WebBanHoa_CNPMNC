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
    public class CheckoutDetailsController : Controller
    {
        private giadinhthoxinhEntities1 db = new giadinhthoxinhEntities1();

        // GET: Admin/CheckoutDetails
        public ActionResult Index()
        {
            var tblCheckoutDetails = db.tblCheckoutDetails.Include(t => t.tblOrder).Include(t => t.tblProduct);
            return View(tblCheckoutDetails.ToList());
        }

        // GET: Admin/CheckoutDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblCheckoutDetail tblCheckoutDetail = db.tblCheckoutDetails.Find(id);
            if (tblCheckoutDetail == null)
            {
                return HttpNotFound();
            }
            return View(tblCheckoutDetail);
        }

        // GET: Admin/CheckoutDetails/Create
        public ActionResult Create()
        {
            ViewBag.FK_iOrderID = new SelectList(db.tblOrders, "PK_iOrderID", "sCustomerName");
            ViewBag.FK_iProductID = new SelectList(db.tblProducts, "PK_iProductID", "sProductName");
            return View();
        }

        // POST: Admin/CheckoutDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PK_iCheckoutDetailID,FK_iOrderID,FK_iProductID,iQuantity,fPrice")] tblCheckoutDetail tblCheckoutDetail)
        {
            if (ModelState.IsValid)
            {
                db.tblCheckoutDetails.Add(tblCheckoutDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FK_iOrderID = new SelectList(db.tblOrders, "PK_iOrderID", "sCustomerName", tblCheckoutDetail.FK_iOrderID);
            ViewBag.FK_iProductID = new SelectList(db.tblProducts, "PK_iProductID", "sProductName", tblCheckoutDetail.FK_iProductID);
            return View(tblCheckoutDetail);
        }

        // GET: Admin/CheckoutDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblCheckoutDetail tblCheckoutDetail = db.tblCheckoutDetails.Find(id);
            if (tblCheckoutDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.FK_iOrderID = new SelectList(db.tblOrders, "PK_iOrderID", "sCustomerName", tblCheckoutDetail.FK_iOrderID);
            ViewBag.FK_iProductID = new SelectList(db.tblProducts, "PK_iProductID", "sProductName", tblCheckoutDetail.FK_iProductID);
            return View(tblCheckoutDetail);
        }

        // POST: Admin/CheckoutDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PK_iCheckoutDetailID,FK_iOrderID,FK_iProductID,iQuantity,fPrice")] tblCheckoutDetail tblCheckoutDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblCheckoutDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FK_iOrderID = new SelectList(db.tblOrders, "PK_iOrderID", "sCustomerName", tblCheckoutDetail.FK_iOrderID);
            ViewBag.FK_iProductID = new SelectList(db.tblProducts, "PK_iProductID", "sProductName", tblCheckoutDetail.FK_iProductID);
            return View(tblCheckoutDetail);
        }

        // GET: Admin/CheckoutDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblCheckoutDetail tblCheckoutDetail = db.tblCheckoutDetails.Find(id);
            if (tblCheckoutDetail == null)
            {
                return HttpNotFound();
            }
            return View(tblCheckoutDetail);
        }

        // POST: Admin/CheckoutDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblCheckoutDetail tblCheckoutDetail = db.tblCheckoutDetails.Find(id);
            db.tblCheckoutDetails.Remove(tblCheckoutDetail);
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
