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
    public class ReviewsController : Controller
    {
        private giadinhthoxinhEntities1 db = new giadinhthoxinhEntities1();

        // GET: Admin/Reviews
        public ActionResult Index()
        {
            var tblReviews = db.tblReviews.Include(t => t.tblProduct).Include(t => t.tblUser);
            return View(tblReviews.ToList());
        }

        // GET: Admin/Reviews/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblReview tblReview = db.tblReviews.Find(id);
            if (tblReview == null)
            {
                return HttpNotFound();
            }
            return View(tblReview);
        }

        // GET: Admin/Reviews/Create
        public ActionResult Create()
        {
            ViewBag.FK_iProductID = new SelectList(db.tblProducts, "PK_iProductID", "sProductName");
            ViewBag.FK_iAccountID = new SelectList(db.tblUsers, "PK_iAccountID", "sEmail");
            return View();
        }

        // POST: Admin/Reviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PK_iReviewID,FK_iProductID,FK_iAccountID,iStarRating,dtReviewTime")] tblReview tblReview)
        {
            if (ModelState.IsValid)
            {
                db.tblReviews.Add(tblReview);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FK_iProductID = new SelectList(db.tblProducts, "PK_iProductID", "sProductName", tblReview.FK_iProductID);
            ViewBag.FK_iAccountID = new SelectList(db.tblUsers, "PK_iAccountID", "sEmail", tblReview.FK_iAccountID);
            return View(tblReview);
        }

        // GET: Admin/Reviews/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblReview tblReview = db.tblReviews.Find(id);
            if (tblReview == null)
            {
                return HttpNotFound();
            }
            ViewBag.FK_iProductID = new SelectList(db.tblProducts, "PK_iProductID", "sProductName", tblReview.FK_iProductID);
            ViewBag.FK_iAccountID = new SelectList(db.tblUsers, "PK_iAccountID", "sEmail", tblReview.FK_iAccountID);
            return View(tblReview);
        }

        // POST: Admin/Reviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PK_iReviewID,FK_iProductID,FK_iAccountID,iStarRating,dtReviewTime")] tblReview tblReview)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblReview).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FK_iProductID = new SelectList(db.tblProducts, "PK_iProductID", "sProductName", tblReview.FK_iProductID);
            ViewBag.FK_iAccountID = new SelectList(db.tblUsers, "PK_iAccountID", "sEmail", tblReview.FK_iAccountID);
            return View(tblReview);
        }

        // GET: Admin/Reviews/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblReview tblReview = db.tblReviews.Find(id);
            if (tblReview == null)
            {
                return HttpNotFound();
            }
            return View(tblReview);
        }

        // POST: Admin/Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblReview tblReview = db.tblReviews.Find(id);
            db.tblReviews.Remove(tblReview);
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
