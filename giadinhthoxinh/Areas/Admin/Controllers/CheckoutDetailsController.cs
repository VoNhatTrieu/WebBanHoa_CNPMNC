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
    public class CheckoutDetailsController : Controller
    {
        private giadinhthoxinhEntities1 db = new giadinhthoxinhEntities1();

        // GET: Admin/CheckoutDetails
        public ActionResult Index()
        {
            var tblCheckoutDetails = db.tblCheckoutDetails.Include(t => t.tblOrder).Include(t => t.tblProduct);
            return View(tblCheckoutDetails.ToList());
        }

        // GET: Admin/CheckoutDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblCheckoutDetail tblCheckoutDetail = db.tblCheckoutDetails.Find(id);
            if (tblCheckoutDetail == null)
            {
                return HttpNotFound();
            }
            return View(tblCheckoutDetail);
        }

        // GET: Admin/CheckoutDetails/Create
        public ActionResult Create()
        {
            ViewBag.FK_iOrderID = new SelectList(db.tblOrders, "PK_iOrderID", "sCustomerName");
            ViewBag.FK_iProductID = new SelectList(db.tblProducts, "PK_iProductID", "sProductName");
            return View();
        }

        // POST: Admin/CheckoutDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PK_iCheckoutDetailID,FK_iOrderID,FK_iProductID,iQuantity,fPrice")] tblCheckoutDetail tblCheckoutDetail)
        {
            if (ModelState.IsValid)
            {
                db.tblCheckoutDetails.Add(tblCheckoutDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FK_iOrderID = new SelectList(db.tblOrders, "PK_iOrderID", "sCustomerName", tblCheckoutDetail.FK_iOrderID);
            ViewBag.FK_iProductID = new SelectList(db.tblProducts, "PK_iProductID", "sProductName", tblCheckoutDetail.FK_iProductID);
            return View(tblCheckoutDetail);
        }

        // GET: Admin/CheckoutDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblCheckoutDetail tblCheckoutDetail = db.tblCheckoutDetails.Find(id);
            if (tblCheckoutDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.FK_iOrderID = new SelectList(db.tblOrders, "PK_iOrderID", "sCustomerName", tblCheckoutDetail.FK_iOrderID);
            ViewBag.FK_iProductID = new SelectList(db.tblProducts, "PK_iProductID", "sProductName", tblCheckoutDetail.FK_iProductID);
            return View(tblCheckoutDetail);
        }

        // POST: Admin/CheckoutDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PK_iCheckoutDetailID,FK_iOrderID,FK_iProductID,iQuantity,fPrice")] tblCheckoutDetail tblCheckoutDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblCheckoutDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FK_iOrderID = new SelectList(db.tblOrders, "PK_iOrderID", "sCustomerName", tblCheckoutDetail.FK_iOrderID);
            ViewBag.FK_iProductID = new SelectList(db.tblProducts, "PK_iProductID", "sProductName", tblCheckoutDetail.FK_iProductID);
            return View(tblCheckoutDetail);
        }

        // GET: Admin/CheckoutDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblCheckoutDetail tblCheckoutDetail = db.tblCheckoutDetails.Find(id);
            if (tblCheckoutDetail == null)
            {
                return HttpNotFound();
            }
            return View(tblCheckoutDetail);
        }

        // POST: Admin/CheckoutDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblCheckoutDetail tblCheckoutDetail = db.tblCheckoutDetails.Find(id);
            db.tblCheckoutDetails.Remove(tblCheckoutDetail);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // Xác nhận sản phẩm trong hóa đơn - AJAX
        [HttpPost]
        public JsonResult XacNhanSanPham(int id)
        {
            try
            {
                // Kiểm tra quyền (Admin hoặc Nhân viên)
                if (Session["Admin"] == null && Session["NhanVien"] == null)
                {
                    return Json(new { success = false, message = "Bạn không có quyền thực hiện thao tác này!" });
                }

                var checkoutDetail = db.tblCheckoutDetails.Find(id);
                if (checkoutDetail == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy chi tiết hóa đơn!" });
                }

                // Kiểm tra trạng thái hiện tại (linh hoạt với encoding)
                if (checkoutDetail.sStatus != null && checkoutDetail.sStatus.Contains("Đã"))
                {
                    return Json(new { success = false, message = "Sản phẩm này đã được xác nhận trước đó!" });
                }

                // Cập nhật trạng thái
                checkoutDetail.sStatus = "Đã xác nhận";
                db.Entry(checkoutDetail).State = EntityState.Modified;
                db.SaveChanges();

                // Kiểm tra xem tất cả sản phẩm trong đơn hàng đã được xác nhận chưa
                var orderId = checkoutDetail.FK_iOrderID;
                var allDetailsConfirmed = db.tblCheckoutDetails
                    .Where(cd => cd.FK_iOrderID == orderId)
                    .All(cd => cd.sStatus != null && cd.sStatus.Contains("Đã"));

                // Nếu tất cả sản phẩm đã được xác nhận, cập nhật trạng thái đơn hàng
                if (allDetailsConfirmed)
                {
                    var order = db.tblOrders.Find(orderId);
                    if (order != null)
                    {
                        order.sState = "Đã xác nhận";
                        db.Entry(order).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }

                // Lấy thông tin người thực hiện
                tblUser user = Session["Admin"] != null ? (tblUser)Session["Admin"] : (tblUser)Session["NhanVien"];

                return Json(new
                {
                    success = true,
                    message = "✅ Xác nhận sản phẩm thành công!",
                    newStatus = "Đã xác nhận",
                    confirmedBy = user.sUserName,
                    confirmedTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
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
