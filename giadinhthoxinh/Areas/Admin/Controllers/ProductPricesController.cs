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
    public class ProductPricesController : Controller
    {
        private giadinhthoxinhEntities1 db = new giadinhthoxinhEntities1();

        // GET: Admin/ProductPrices
        public ActionResult Index()
        {
            var tblProductPrices = db.tblProductPrices.Include(t => t.tblProduct);
            return View(tblProductPrices.ToList());
        }

        // GET: Admin/ProductPrices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblProductPrice tblProductPrice = db.tblProductPrices.Find(id);
            if (tblProductPrice == null)
            {
                return HttpNotFound();
            }
            return View(tblProductPrice);
        }

        // GET: Admin/ProductPrices/Create
        public ActionResult Create()
        {
            ViewBag.FK_iProductID = new SelectList(db.tblProducts, "PK_iProductID", "sProductName");
            return View();
        }

        // POST: Admin/ProductPrices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PK_iProductPriceID,FK_iProductID,fPrice,dtStartDay,dtEndDay")] tblProductPrice tblProductPrice)
        {
            if (ModelState.IsValid)
            {
                db.tblProductPrices.Add(tblProductPrice);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FK_iProductID = new SelectList(db.tblProducts, "PK_iProductID", "sProductName", tblProductPrice.FK_iProductID);
            return View(tblProductPrice);
        }

        // GET: Admin/ProductPrices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblProductPrice tblProductPrice = db.tblProductPrices.Find(id);
            if (tblProductPrice == null)
            {
                return HttpNotFound();
            }
            ViewBag.FK_iProductID = new SelectList(db.tblProducts, "PK_iProductID", "sProductName", tblProductPrice.FK_iProductID);
            return View(tblProductPrice);
        }

        // POST: Admin/ProductPrices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PK_iProductPriceID,FK_iProductID,fPrice,dtStartDay,dtEndDay")] tblProductPrice tblProductPrice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblProductPrice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FK_iProductID = new SelectList(db.tblProducts, "PK_iProductID", "sProductName", tblProductPrice.FK_iProductID);
            return View(tblProductPrice);
        }

        // GET: Admin/ProductPrices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblProductPrice tblProductPrice = db.tblProductPrices.Find(id);
            if (tblProductPrice == null)
            {
                return HttpNotFound();
            }
            return View(tblProductPrice);
        }

        // POST: Admin/ProductPrices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblProductPrice tblProductPrice = db.tblProductPrices.Find(id);
            db.tblProductPrices.Remove(tblProductPrice);
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
