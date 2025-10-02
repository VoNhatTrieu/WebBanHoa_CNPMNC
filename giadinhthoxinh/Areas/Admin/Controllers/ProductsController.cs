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
    public class ProductsController : Controller
    {
        private giadinhthoxinhEntities1 db = new giadinhthoxinhEntities1();

        // GET: Admin/Products
        public ActionResult Index()
        {
            var tblProducts = db.tblProducts.Include(t => t.tblCategory).Include(t => t.tblPromote);
            return View(tblProducts.ToList());
        }

        // GET: Admin/Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblProduct tblProduct = db.tblProducts.Find(id);
            if (tblProduct == null)
            {
                return HttpNotFound();
            }
            return View(tblProduct);
        }

        // GET: Admin/Products/Create
        public ActionResult Create()
        {
            ViewBag.FK_iCategoryID = new SelectList(db.tblCategories, "PK_iCategoryID", "sCategoryName");
            ViewBag.FK_iPromoteID = new SelectList(db.tblPromotes, "PK_iPromoteID", "sPromoteName");
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PK_iProductID,FK_iCategoryID,FK_iPromoteID,sProductName,sDescribe,fPrice,sColor,sSize,sImage,sUnit")] tblProduct tblProduct, HttpPostedFileBase fileAnh)
        {
            if (ModelState.IsValid && fileAnh.ContentLength > 0)
            {
                //Lưu file
                string rootFolder = Server.MapPath("/Data/");
                string pathImage = rootFolder + fileAnh.FileName;
                fileAnh.SaveAs(pathImage);
                //Lưu url hình ảnh
                tblProduct.sImage = "/Data/" + fileAnh.FileName;

                db.tblProducts.Add(tblProduct);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //else
            //{
            //    ModelState.AddModelError("FileAnh", "Ảnh không được để trống");
            //}

            ViewBag.FK_iCategoryID = new SelectList(db.tblCategories, "PK_iCategoryID", "sCategoryName", tblProduct.FK_iCategoryID);
            ViewBag.FK_iPromoteID = new SelectList(db.tblPromotes, "PK_iPromoteID", "sPromoteName", tblProduct.FK_iPromoteID);
            return View(tblProduct);
        }

        // GET: Admin/Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblProduct tblProduct = db.tblProducts.Find(id);
            if (tblProduct == null)
            {
                return HttpNotFound();
            }
            ViewBag.FK_iCategoryID = new SelectList(db.tblCategories, "PK_iCategoryID", "sCategoryName", tblProduct.FK_iCategoryID);
            ViewBag.FK_iPromoteID = new SelectList(db.tblPromotes, "PK_iPromoteID", "sPromoteName", tblProduct.FK_iPromoteID);
            return View(tblProduct);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PK_iProductID,FK_iCategoryID,FK_iPromoteID,sProductName,sDescribe,fPrice,sColor,sSize,sImage,sUnit")] tblProduct tblProduct, HttpPostedFileBase fileAnh)
        {
            if (ModelState.IsValid)
            {
                //Lưu file
                string rootFolder = Server.MapPath("/Data/");
                string pathImage = rootFolder + fileAnh.FileName;
                fileAnh.SaveAs(pathImage);
                //Lưu url hình ảnh
                tblProduct.sImage = "/Data/" + fileAnh.FileName;


                db.Entry(tblProduct).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FK_iCategoryID = new SelectList(db.tblCategories, "PK_iCategoryID", "sCategoryName", tblProduct.FK_iCategoryID);
            ViewBag.FK_iPromoteID = new SelectList(db.tblPromotes, "PK_iPromoteID", "sPromoteName", tblProduct.FK_iPromoteID);
            return View(tblProduct);
        }

        // GET: Admin/Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblProduct tblProduct = db.tblProducts.Find(id);
            if (tblProduct == null)
            {
                return HttpNotFound();
            }
            return View(tblProduct);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblProduct tblProduct = db.tblProducts.Find(id);
            db.tblProducts.Remove(tblProduct);
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
