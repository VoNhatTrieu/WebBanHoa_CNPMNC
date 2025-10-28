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
        public ActionResult Create(HttpPostedFileBase fileAnh)
        {
            try
            {
                tblProduct product = new tblProduct();

                // Lấy các giá trị từ form
                int.TryParse(Request.Form["FK_iCategoryID"], out int catId);
                int.TryParse(Request.Form["FK_iPromoteID"], out int promoId);

                product.FK_iCategoryID = catId;
                product.FK_iPromoteID = promoId;
                product.sProductName = Request.Form["sProductName"];
                product.sDescribe = Request.Form["sDescribe"];
                product.sColor = Request.Form["sColor"];
                product.sSize = Request.Form["sSize"];
                product.sUnit = Request.Form["sUnit"];

                // ✅ Làm sạch giá tiền
                string rawPrice = Request.Form["fPrice"];
                if (!string.IsNullOrEmpty(rawPrice))
                {
                    rawPrice = rawPrice.Replace(".", "").Replace(",", "");
                    if (float.TryParse(rawPrice, out float priceValue))
                        product.fPrice = priceValue;
                    else
                        ModelState.AddModelError("fPrice", "Giá không hợp lệ.");
                }

                // ✅ Xử lý upload ảnh
                if (fileAnh != null && fileAnh.ContentLength > 0)
                {
                    string rootFolder = Server.MapPath("/Data/");
                    string fileName = System.IO.Path.GetFileName(fileAnh.FileName);
                    string pathImage = System.IO.Path.Combine(rootFolder, fileName);
                    fileAnh.SaveAs(pathImage);
                    product.sImage = "/Data/" + fileName;
                }

                if (ModelState.IsValid)
                {
                    db.tblProducts.Add(product);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Lỗi: " + ex.Message);
            }

            ViewBag.FK_iCategoryID = new SelectList(db.tblCategories, "PK_iCategoryID", "sCategoryName");
            ViewBag.FK_iPromoteID = new SelectList(db.tblPromotes, "PK_iPromoteID", "sPromoteName");
            return View();
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
