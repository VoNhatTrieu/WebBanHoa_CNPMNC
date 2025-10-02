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
    public class CategoriesController : Controller
    {
        private giadinhthoxinhEntities1 db = new giadinhthoxinhEntities1();

        // GET: Admin/Categories
        public ActionResult Index()
        {
            return View(db.tblCategories.ToList());
        }

        // GET: Admin/Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblCategory tblCategory = db.tblCategories.Find(id);
            if (tblCategory == null)
            {
                return HttpNotFound();
            }
            return View(tblCategory);
        }

        // GET: Admin/Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PK_iCategoryID,sCategoryName")] tblCategory tblCategory)
        {
            if (ModelState.IsValid)
            {
                db.tblCategories.Add(tblCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblCategory);
        }

        // GET: Admin/Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblCategory tblCategory = db.tblCategories.Find(id);
            if (tblCategory == null)
            {
                return HttpNotFound();
            }
            return View(tblCategory);
        }

        // POST: Admin/Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PK_iCategoryID,sCategoryName")] tblCategory tblCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblCategory);
        }

        // GET: Admin/Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblCategory tblCategory = db.tblCategories.Find(id);
            if (tblCategory == null)
            {
                return HttpNotFound();
            }
            return View(tblCategory);
        }

        // POST: Admin/Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblCategory tblCategory = db.tblCategories.Find(id);
            db.tblCategories.Remove(tblCategory);
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
