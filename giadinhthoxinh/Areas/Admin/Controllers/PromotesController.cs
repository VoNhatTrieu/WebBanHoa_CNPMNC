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
    public class PromotesController : Controller
    {
        private giadinhthoxinhEntities1 db = new giadinhthoxinhEntities1();

        // GET: Admin/Promotes
        public ActionResult Index()
        {
            return View(db.tblPromotes.ToList());
        }

        // GET: Admin/Promotes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblPromote tblPromote = db.tblPromotes.Find(id);
            if (tblPromote == null)
            {
                return HttpNotFound();
            }
            return View(tblPromote);
        }

        // GET: Admin/Promotes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Promotes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PK_iPromoteID,sPromoteName,sPromoteRate,dtStartDay,dtEndDay")] tblPromote tblPromote)
        {
            if (ModelState.IsValid)
            {
                db.tblPromotes.Add(tblPromote);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblPromote);
        }

        // GET: Admin/Promotes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblPromote tblPromote = db.tblPromotes.Find(id);
            if (tblPromote == null)
            {
                return HttpNotFound();
            }
            return View(tblPromote);
        }

        // POST: Admin/Promotes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PK_iPromoteID,sPromoteName,sPromoteRate,dtStartDay,dtEndDay")] tblPromote tblPromote)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblPromote).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblPromote);
        }

        // GET: Admin/Promotes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblPromote tblPromote = db.tblPromotes.Find(id);
            if (tblPromote == null)
            {
                return HttpNotFound();
            }
            return View(tblPromote);
        }

        // POST: Admin/Promotes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblPromote tblPromote = db.tblPromotes.Find(id);
            db.tblPromotes.Remove(tblPromote);
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
