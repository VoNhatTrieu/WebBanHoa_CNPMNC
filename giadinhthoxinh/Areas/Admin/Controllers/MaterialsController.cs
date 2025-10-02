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
    public class MaterialsController : Controller
    {
        private giadinhthoxinhEntities1 db = new giadinhthoxinhEntities1();

        // GET: Admin/Materials
        public ActionResult Index()
        {
            return View(db.tblMaterials.ToList());
        }

        // GET: Admin/Materials/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblMaterial tblMaterial = db.tblMaterials.Find(id);
            if (tblMaterial == null)
            {
                return HttpNotFound();
            }
            return View(tblMaterial);
        }

        // GET: Admin/Materials/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Materials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PK_iMaterialID,sMaterialName,sDescribe,iQuatity,sUnit")] tblMaterial tblMaterial)
        {
            if (ModelState.IsValid)
            {
                db.tblMaterials.Add(tblMaterial);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblMaterial);
        }

        // GET: Admin/Materials/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblMaterial tblMaterial = db.tblMaterials.Find(id);
            if (tblMaterial == null)
            {
                return HttpNotFound();
            }
            return View(tblMaterial);
        }

        // POST: Admin/Materials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PK_iMaterialID,sMaterialName,sDescribe,iQuatity,sUnit")] tblMaterial tblMaterial)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblMaterial).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblMaterial);
        }

        // GET: Admin/Materials/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblMaterial tblMaterial = db.tblMaterials.Find(id);
            if (tblMaterial == null)
            {
                return HttpNotFound();
            }
            return View(tblMaterial);
        }

        // POST: Admin/Materials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblMaterial tblMaterial = db.tblMaterials.Find(id);
            db.tblMaterials.Remove(tblMaterial);
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
