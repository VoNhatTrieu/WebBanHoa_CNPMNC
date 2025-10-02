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
    public class ImportMaterialsController : Controller
    {
        private giadinhthoxinhEntities1 db = new giadinhthoxinhEntities1();

        // GET: Admin/ImportMaterials
        public ActionResult Index()
        {
            var tblImportMaterials = db.tblImportMaterials.Include(t => t.tblImportOrder).Include(t => t.tblMaterial);
            return View(tblImportMaterials.ToList());
        }

        // GET: Admin/ImportMaterials/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblImportMaterial tblImportMaterial = db.tblImportMaterials.Find(id);
            if (tblImportMaterial == null)
            {
                return HttpNotFound();
            }
            return View(tblImportMaterial);
        }

        // GET: Admin/ImportMaterials/Create
        public ActionResult Create()
        {
            ViewBag.FK_iImportOrderID = new SelectList(db.tblImportOrders, "PK_iImportOrderID", "sDeliver");
            ViewBag.FK_iMaterialID = new SelectList(db.tblMaterials, "PK_iMaterialID", "sMaterialName");
            return View();
        }

        // POST: Admin/ImportMaterials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PK_iImportMaterialID,FK_iImportOrderID,FK_iMaterialID,iQuatity,fPrice")] tblImportMaterial tblImportMaterial)
        {
            if (ModelState.IsValid)
            {
                db.tblImportMaterials.Add(tblImportMaterial);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FK_iImportOrderID = new SelectList(db.tblImportOrders, "PK_iImportOrderID", "sDeliver", tblImportMaterial.FK_iImportOrderID);
            ViewBag.FK_iMaterialID = new SelectList(db.tblMaterials, "PK_iMaterialID", "sMaterialName", tblImportMaterial.FK_iMaterialID);
            return View(tblImportMaterial);
        }

        // GET: Admin/ImportMaterials/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblImportMaterial tblImportMaterial = db.tblImportMaterials.Find(id);
            if (tblImportMaterial == null)
            {
                return HttpNotFound();
            }
            ViewBag.FK_iImportOrderID = new SelectList(db.tblImportOrders, "PK_iImportOrderID", "sDeliver", tblImportMaterial.FK_iImportOrderID);
            ViewBag.FK_iMaterialID = new SelectList(db.tblMaterials, "PK_iMaterialID", "sMaterialName", tblImportMaterial.FK_iMaterialID);
            return View(tblImportMaterial);
        }

        // POST: Admin/ImportMaterials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PK_iImportMaterialID,FK_iImportOrderID,FK_iMaterialID,iQuatity,fPrice")] tblImportMaterial tblImportMaterial)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblImportMaterial).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FK_iImportOrderID = new SelectList(db.tblImportOrders, "PK_iImportOrderID", "sDeliver", tblImportMaterial.FK_iImportOrderID);
            ViewBag.FK_iMaterialID = new SelectList(db.tblMaterials, "PK_iMaterialID", "sMaterialName", tblImportMaterial.FK_iMaterialID);
            return View(tblImportMaterial);
        }

        // GET: Admin/ImportMaterials/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblImportMaterial tblImportMaterial = db.tblImportMaterials.Find(id);
            if (tblImportMaterial == null)
            {
                return HttpNotFound();
            }
            return View(tblImportMaterial);
        }

        // POST: Admin/ImportMaterials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblImportMaterial tblImportMaterial = db.tblImportMaterials.Find(id);
            db.tblImportMaterials.Remove(tblImportMaterial);
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
