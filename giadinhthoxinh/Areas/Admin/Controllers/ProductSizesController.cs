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
    public class ProductSizesController : Controller
    {
        private giadinhthoxinhEntities1 db = new giadinhthoxinhEntities1();

        // GET: Admin/ProductSizes
        public ActionResult Index()
        {
            var tblProductSizes = db.tblProductSizes.Include(t => t.tblProduct);
            return View(tblProductSizes.ToList());
        }

        // GET: Admin/ProductSizes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblProductSize tblProductSize = db.tblProductSizes.Find(id);
            if (tblProductSize == null)
            {
                return HttpNotFound();
            }
            return View(tblProductSize);
        }

        // GET: Admin/ProductSizes/Create
        public ActionResult Create()
        {
            ViewBag.FK_iProductID = new SelectList(db.tblProducts, "PK_iProductID", "sProductName");
            return View();
        }

        // POST: Admin/ProductSizes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PK_iProductSizeID,FK_iProductID,sSizeName")] tblProductSize tblProductSize)
        {
            if (ModelState.IsValid)
            {
                db.tblProductSizes.Add(tblProductSize);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FK_iProductID = new SelectList(db.tblProducts, "PK_iProductID", "sProductName", tblProductSize.FK_iProductID);
            return View(tblProductSize);
        }

        // GET: Admin/ProductSizes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblProductSize tblProductSize = db.tblProductSizes.Find(id);
            if (tblProductSize == null)
            {
                return HttpNotFound();
            }
            ViewBag.FK_iProductID = new SelectList(db.tblProducts, "PK_iProductID", "sProductName", tblProductSize.FK_iProductID);
            return View(tblProductSize);
        }

        // POST: Admin/ProductSizes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PK_iProductSizeID,FK_iProductID,sSizeName")] tblProductSize tblProductSize)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblProductSize).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FK_iProductID = new SelectList(db.tblProducts, "PK_iProductID", "sProductName", tblProductSize.FK_iProductID);
            return View(tblProductSize);
        }

        // GET: Admin/ProductSizes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblProductSize tblProductSize = db.tblProductSizes.Find(id);
            if (tblProductSize == null)
            {
                return HttpNotFound();
            }
            return View(tblProductSize);
        }

        // POST: Admin/ProductSizes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblProductSize tblProductSize = db.tblProductSizes.Find(id);
            db.tblProductSizes.Remove(tblProductSize);
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
