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
    public class MaterColorsController : Controller
    {
        private giadinhthoxinhEntities1 db = new giadinhthoxinhEntities1();

        // GET: Admin/MaterColors
        public ActionResult Index()
        {
            var tblMaterColors = db.tblMaterColors.Include(t => t.tblMaterial);
            return View(tblMaterColors.ToList());
        }

        // GET: Admin/MaterColors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblMaterColor tblMaterColor = db.tblMaterColors.Find(id);
            if (tblMaterColor == null)
            {
                return HttpNotFound();
            }
            return View(tblMaterColor);
        }

        // GET: Admin/MaterColors/Create
        public ActionResult Create()
        {
            ViewBag.FK_iMaterialID = new SelectList(db.tblMaterials, "PK_iMaterialID", "sMaterialName");
            return View();
        }

        // POST: Admin/MaterColors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PK_iMaterColorID,FK_iMaterialID,sMaterColor")] tblMaterColor tblMaterColor)
        {
            if (ModelState.IsValid)
            {
                db.tblMaterColors.Add(tblMaterColor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FK_iMaterialID = new SelectList(db.tblMaterials, "PK_iMaterialID", "sMaterialName", tblMaterColor.FK_iMaterialID);
            return View(tblMaterColor);
        }

        // GET: Admin/MaterColors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblMaterColor tblMaterColor = db.tblMaterColors.Find(id);
            if (tblMaterColor == null)
            {
                return HttpNotFound();
            }
            ViewBag.FK_iMaterialID = new SelectList(db.tblMaterials, "PK_iMaterialID", "sMaterialName", tblMaterColor.FK_iMaterialID);
            return View(tblMaterColor);
        }

        // POST: Admin/MaterColors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PK_iMaterColorID,FK_iMaterialID,sMaterColor")] tblMaterColor tblMaterColor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblMaterColor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FK_iMaterialID = new SelectList(db.tblMaterials, "PK_iMaterialID", "sMaterialName", tblMaterColor.FK_iMaterialID);
            return View(tblMaterColor);
        }

        // GET: Admin/MaterColors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblMaterColor tblMaterColor = db.tblMaterColors.Find(id);
            if (tblMaterColor == null)
            {
                return HttpNotFound();
            }
            return View(tblMaterColor);
        }

        // POST: Admin/MaterColors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblMaterColor tblMaterColor = db.tblMaterColors.Find(id);
            db.tblMaterColors.Remove(tblMaterColor);
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
