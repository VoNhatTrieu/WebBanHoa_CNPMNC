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
    public class ImagesController : Controller
    {
        private giadinhthoxinhEntities1 db = new giadinhthoxinhEntities1();

        // GET: Admin/Images
        public ActionResult Index()
        {
            var tblImages = db.tblImages.Include(t => t.tblProduct);
            return View(tblImages.ToList());
        }

        // GET: Admin/Images/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblImage tblImage = db.tblImages.Find(id);
            if (tblImage == null)
            {
                return HttpNotFound();
            }
            return View(tblImage);
        }

        // GET: Admin/Images/Create
        public ActionResult Create()
        {
            ViewBag.FK_iProductID = new SelectList(db.tblProducts, "PK_iProductID", "sProductName");
            return View();
        }

        // POST: Admin/Images/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PK_iImageID,FK_iProductID,sImage")] tblImage tblImage, HttpPostedFileBase fileAnh)
        {
            if (fileAnh.ContentLength > 0 && ModelState.IsValid)
            {
                //Lưu file
                string rootFolder = Server.MapPath("/Data/");
                string pathImage = rootFolder + fileAnh.FileName;
                fileAnh.SaveAs(pathImage);
                //Lưu url hình ảnh
                tblImage.sImage = "/Data/" + fileAnh.FileName;

                db.tblImages.Add(tblImage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }


            ViewBag.FK_iProductID = new SelectList(db.tblProducts, "PK_iProductID", "sProductName", tblImage.FK_iProductID);
            return View(tblImage);
        }

        // GET: Admin/Images/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblImage tblImage = db.tblImages.Find(id);
            if (tblImage == null)
            {
                return HttpNotFound();
            }
            ViewBag.FK_iProductID = new SelectList(db.tblProducts, "PK_iProductID", "sProductName", tblImage.FK_iProductID);
            return View(tblImage);
        }

        // POST: Admin/Images/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PK_iImageID,FK_iProductID,sImage")] tblImage tblImage, HttpPostedFileBase fileAnh)
        {
            if (ModelState.IsValid)
            {
                //Lưu file
                string rootFolder = Server.MapPath("/Data/");
                string pathImage = rootFolder + fileAnh.FileName;
                fileAnh.SaveAs(pathImage);
                //Lưu url hình ảnh
                tblImage.sImage = "/Data/" + fileAnh.FileName;

                db.Entry(tblImage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FK_iProductID = new SelectList(db.tblProducts, "PK_iProductID", "sProductName", tblImage.FK_iProductID);
            return View(tblImage);
        }

        // GET: Admin/Images/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblImage tblImage = db.tblImages.Find(id);
            if (tblImage == null)
            {
                return HttpNotFound();
            }
            return View(tblImage);
        }

        // POST: Admin/Images/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblImage tblImage = db.tblImages.Find(id);
            db.tblImages.Remove(tblImage);
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
