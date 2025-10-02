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
    public class MaterPriceImportsController : Controller
    {
        private giadinhthoxinhEntities1 db = new giadinhthoxinhEntities1();

        // GET: Admin/MaterPriceImports
        public ActionResult Index()
        {
            var tblMaterPriceImports = db.tblMaterPriceImports.Include(t => t.tblMaterial);
            return View(tblMaterPriceImports.ToList());
        }

        // GET: Admin/MaterPriceImports/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblMaterPriceImport tblMaterPriceImport = db.tblMaterPriceImports.Find(id);
            if (tblMaterPriceImport == null)
            {
                return HttpNotFound();
            }
            return View(tblMaterPriceImport);
        }

        // GET: Admin/MaterPriceImports/Create
        public ActionResult Create()
        {
            ViewBag.FK_iMaterialID = new SelectList(db.tblMaterials, "PK_iMaterialID", "sMaterialName");
            return View();
        }

        // POST: Admin/MaterPriceImports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PK_iMaterPriceImportID,FK_iMaterialID,fMaterPriceImport,dtStartDay,dtEndDay")] tblMaterPriceImport tblMaterPriceImport)
        {
            if (ModelState.IsValid)
            {
                db.tblMaterPriceImports.Add(tblMaterPriceImport);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FK_iMaterialID = new SelectList(db.tblMaterials, "PK_iMaterialID", "sMaterialName", tblMaterPriceImport.FK_iMaterialID);
            return View(tblMaterPriceImport);
        }

        // GET: Admin/MaterPriceImports/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblMaterPriceImport tblMaterPriceImport = db.tblMaterPriceImports.Find(id);
            if (tblMaterPriceImport == null)
            {
                return HttpNotFound();
            }
            ViewBag.FK_iMaterialID = new SelectList(db.tblMaterials, "PK_iMaterialID", "sMaterialName", tblMaterPriceImport.FK_iMaterialID);
            return View(tblMaterPriceImport);
        }

        // POST: Admin/MaterPriceImports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PK_iMaterPriceImportID,FK_iMaterialID,fMaterPriceImport,dtStartDay,dtEndDay")] tblMaterPriceImport tblMaterPriceImport)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblMaterPriceImport).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FK_iMaterialID = new SelectList(db.tblMaterials, "PK_iMaterialID", "sMaterialName", tblMaterPriceImport.FK_iMaterialID);
            return View(tblMaterPriceImport);
        }

        // GET: Admin/MaterPriceImports/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblMaterPriceImport tblMaterPriceImport = db.tblMaterPriceImports.Find(id);
            if (tblMaterPriceImport == null)
            {
                return HttpNotFound();
            }
            return View(tblMaterPriceImport);
        }

        // POST: Admin/MaterPriceImports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblMaterPriceImport tblMaterPriceImport = db.tblMaterPriceImports.Find(id);
            db.tblMaterPriceImports.Remove(tblMaterPriceImport);
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
