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
    public class ProductColorsController : Controller
    {
        private giadinhthoxinhEntities1 db = new giadinhthoxinhEntities1();

        // GET: Admin/ProductColors
        public ActionResult Index()
        {
            var tblProductColors = db.tblProductColors.Include(t => t.tblProduct);
            return View(tblProductColors.ToList());
        }

        // GET: Admin/ProductColors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblProductColor tblProductColor = db.tblProductColors.Find(id);
            if (tblProductColor == null)
            {
                return HttpNotFound();
            }
            return View(tblProductColor);
        }

        // GET: Admin/ProductColors/Create
        public ActionResult Create()
        {
            ViewBag.FK_iProductID = new SelectList(db.tblProducts, "PK_iProductID", "sProductName");
            return View();
        }

        // POST: Admin/ProductColors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PK_iProductColorID,FK_iProductID,sProductColor")] tblProductColor tblProductColor)
        {
            if (ModelState.IsValid)
            {
                db.tblProductColors.Add(tblProductColor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FK_iProductID = new SelectList(db.tblProducts, "PK_iProductID", "sProductName", tblProductColor.FK_iProductID);
            return View(tblProductColor);
        }

        // GET: Admin/ProductColors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblProductColor tblProductColor = db.tblProductColors.Find(id);
            if (tblProductColor == null)
            {
                return HttpNotFound();
            }
            ViewBag.FK_iProductID = new SelectList(db.tblProducts, "PK_iProductID", "sProductName", tblProductColor.FK_iProductID);
            return View(tblProductColor);
        }

        // POST: Admin/ProductColors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PK_iProductColorID,FK_iProductID,sProductColor")] tblProductColor tblProductColor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblProductColor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FK_iProductID = new SelectList(db.tblProducts, "PK_iProductID", "sProductName", tblProductColor.FK_iProductID);
            return View(tblProductColor);
        }

        // GET: Admin/ProductColors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblProductColor tblProductColor = db.tblProductColors.Find(id);
            if (tblProductColor == null)
            {
                return HttpNotFound();
            }
            return View(tblProductColor);
        }

        // POST: Admin/ProductColors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblProductColor tblProductColor = db.tblProductColors.Find(id);
            db.tblProductColors.Remove(tblProductColor);
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
