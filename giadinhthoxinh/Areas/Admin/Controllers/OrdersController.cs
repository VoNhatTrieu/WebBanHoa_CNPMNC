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
    public class OrdersController : Controller
    {
        private giadinhthoxinhEntities1 db = new giadinhthoxinhEntities1();

        // GET: Admin/Orders
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SearchDonHang(string searchString)
        {
            Session["SearchDonHang"] = searchString;
            IOrderedQueryable<tblOrder> model = (IOrderedQueryable<tblOrder>)db.tblOrders.OrderByDescending(x => x.dInvoidDate);
            if (!String.IsNullOrEmpty(searchString))
            {
                model = (IOrderedQueryable<tblOrder>)model.Where(x => x.sCustomerPhone.Contains(searchString));
            }
            return View(model);
        }

        // Xác nhận đơn hàng - Chuyển từ "Chờ xác nhận" sang "Đã xác nhận"
        [HttpPost]
        public JsonResult XacNhanDonHang(int id)
        {
            try
            {
                var order = db.tblOrders.Find(id);
                if (order == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy đơn hàng!" });
                }

                // Kiểm tra trạng thái đơn hàng
                if (order.sState != "Chờ xác nhận")
                {
                    return Json(new { success = false, message = "Đơn hàng đã được xác nhận trước đó!" });
                }

                // Kiểm tra quyền (Admin hoặc Nhân viên)
                if (Session["Admin"] == null && Session["NhanVien"] == null)
                {
                    return Json(new { success = false, message = "Bạn không có quyền thực hiện thao tác này!" });
                }

                // Lấy thông tin người thực hiện
                tblUser user = null;
                if (Session["Admin"] != null)
                {
                    user = (tblUser)Session["Admin"];
                }
                else if (Session["NhanVien"] != null)
                {
                    user = (tblUser)Session["NhanVien"];
                }

                // Cập nhật trạng thái
                order.sState = "Đã xác nhận";
                order.sBiller = user.sUserName;
                order.dInvoidDate = DateTime.Now; // Cập nhật thời gian xác nhận
                
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();

                return Json(new { 
                    success = true, 
                    message = "✅ Xác nhận đơn hàng thành công!", 
                    newState = "Đã xác nhận",
                    confirmedBy = user.sUserName,
                    confirmedTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
        }

        public ActionResult DuyetDonHang(int id)
        {
            var item = db.tblOrders.Find(id);
            if (Session["NhanVien"] != null)
            {
                tblUser nv = (tblUser)Session["NhanVien"];
                item.sBiller = nv.sUserName;
                item.sState = "Đang giao hàng";
                db.SaveChanges();
            }
            else
            {
                return RedirectToAction("KhongDuThamQuyen", "PhanQuyen");
            }
            return RedirectToAction("Index");
        }

        public ActionResult HoanThanhDonHang(int id)
        {
            var item = db.tblOrders.Find(id);
            if (Session["NhanVien"] != null)
            {
                tblUser nv = (tblUser)Session["NhanVien"];
                item.sBiller = nv.sUserName;
                item.sState = "Hoàn thành";
                db.SaveChanges();
            }
            else
            {
                return RedirectToAction("YeuCauDangNhap", "Login");
            }
            return RedirectToAction("Index");
        }

        public ActionResult HuyDonHang(int id)
        {
            var item = db.tblOrders.Find(id);
            if (Session["NhanVien"] != null)
            {
                tblUser nv = (tblUser)Session["NhanVien"];
                item.sBiller = nv.sUserName;
                item.sState = "Đã hủy";
                db.SaveChanges();
            }
            else
            {
                return RedirectToAction("YeuCauDangNhap", "Login");
            }
            return RedirectToAction("Index");
        }

        public ActionResult DonHangChuaXuLy_Partial()
        {
            var it = db.tblOrders.Where(n => n.sState == "Chờ xác nhận").OrderByDescending(x => x.dInvoidDate);
            return PartialView(it);
        }

        public ActionResult DonHangDangGiao_Partial()
        {
            var it = db.tblOrders.Where(n => n.sState == "Đang giao hàng").OrderByDescending(x => x.dInvoidDate);
            return PartialView(it);
        }

        public ActionResult DonHangDaXuLy_Partial()
        {
            var it = db.tblOrders.Where(n => n.sState == "Hoàn thành" || n.sState == "Đã hủy").OrderByDescending(x => x.dInvoidDate);
            return PartialView(it);
        }

        // API mới: Lấy trạng thái tất cả đơn hàng (cho admin auto-refresh)
        [HttpGet]
        public JsonResult GetAllOrderStatusesForAdmin()
        {
            try
            {
                // Kiểm tra quyền (Admin hoặc Nhân viên)
                if (Session["Admin"] == null && Session["NhanVien"] == null)
                {
                    return Json(new { success = false, message = "Bạn không có quyền truy cập!" }, JsonRequestBehavior.AllowGet);
                }

                var orderStatuses = db.tblOrders
                    .OrderByDescending(o => o.dInvoidDate)
                    .Select(o => new
                    {
                        orderId = o.PK_iOrderID,
                        status = o.sState ?? "Chờ xác nhận",
                        biller = o.sBiller,
                        invoiceDate = o.dInvoidDate,
                        paid = o.iPaid
                    })
                    .ToList();

                return Json(new { success = true, orders = orderStatuses }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // API mới: Lấy đơn hàng chưa xử lý
        [HttpGet]
        public JsonResult GetPendingOrders()
        {
            try
            {
                if (Session["Admin"] == null && Session["NhanVien"] == null)
                {
                    return Json(new { success = false, message = "Bạn không có quyền truy cập!" }, JsonRequestBehavior.AllowGet);
                }

                var pendingOrders = db.tblOrders
                    .Where(n => n.sState == "Chờ xác nhận")
                    .OrderByDescending(x => x.dInvoidDate)
                    .Select(o => new
                    {
                        orderId = o.PK_iOrderID,
                        status = o.sState ?? "Chờ xác nhận"
                    })
                    .ToList();

                return Json(new { success = true, orders = pendingOrders }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // API mới: Lấy đơn hàng đang giao
        [HttpGet]
        public JsonResult GetShippingOrders()
        {
            try
            {
                if (Session["Admin"] == null && Session["NhanVien"] == null)
                {
                    return Json(new { success = false, message = "Bạn không có quyền truy cập!" }, JsonRequestBehavior.AllowGet);
                }

                var shippingOrders = db.tblOrders
                    .Where(n => n.sState == "Đang giao hàng")
                    .OrderByDescending(x => x.dInvoidDate)
                    .Select(o => new
                    {
                        orderId = o.PK_iOrderID,
                        status = o.sState ?? "Đang giao hàng"
                    })
                    .ToList();

                return Json(new { success = true, orders = shippingOrders }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // API mới: Lấy đơn hàng đã xử lý
        [HttpGet]
        public JsonResult GetProcessedOrders()
        {
            try
            {
                if (Session["Admin"] == null && Session["NhanVien"] == null)
                {
                    return Json(new { success = false, message = "Bạn không có quyền truy cập!" }, JsonRequestBehavior.AllowGet);
                }

                var processedOrders = db.tblOrders
                    .Where(n => n.sState == "Hoàn thành" || n.sState == "Đã hủy")
                    .OrderByDescending(x => x.dInvoidDate)
                    .Select(o => new
                    {
                        orderId = o.PK_iOrderID,
                        status = o.sState ?? "Hoàn thành"
                    })
                    .ToList();

                return Json(new { success = true, orders = processedOrders }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Admin/Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblOrder tblOrder = db.tblOrders.Find(id);
            if (tblOrder == null)
            {
                return HttpNotFound();
            }
            return View(tblOrder);
        }

        // GET: Admin/Orders/Create
        public ActionResult Create()
        {
            ViewBag.FK_iAccountID = new SelectList(db.tblUsers, "PK_iAccountID", "sEmail");
            return View();
        }

        // POST: Admin/Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PK_iOrderID,FK_iAccountID,sCustomerName,sCustomerPhone,sDeliveryAddress,dInvoidDate,sBiller,iDeliveryMethod,fSurcharge,iPaid,sState")] tblOrder tblOrder)
        {
            if (ModelState.IsValid)
            {
                // Gán trạng thái mặc định cho đơn mới
                tblOrder.sState = "Chờ xác nhận";
                tblOrder.dInvoidDate = DateTime.Now;

                db.tblOrders.Add(tblOrder);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FK_iAccountID = new SelectList(db.tblUsers, "PK_iAccountID", "sEmail", tblOrder.FK_iAccountID);
            return View(tblOrder);
        }

        // GET: Admin/Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblOrder tblOrder = db.tblOrders.Find(id);
            if (tblOrder == null)
            {
                return HttpNotFound();
            }
            ViewBag.FK_iAccountID = new SelectList(db.tblUsers, "PK_iAccountID", "sEmail", tblOrder.FK_iAccountID);
            return View(tblOrder);
        }

        // POST: Admin/Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PK_iOrderID,FK_iAccountID,sCustomerName,sCustomerPhone,sDeliveryAddress,dInvoidDate,sBiller,iDeliveryMethod,fSurcharge,iPaid,sState")] tblOrder tblOrder)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblOrder).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FK_iAccountID = new SelectList(db.tblUsers, "PK_iAccountID", "sEmail", tblOrder.FK_iAccountID);
            return View(tblOrder);
        }

        // GET: Admin/Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblOrder tblOrder = db.tblOrders.Find(id);
            if (tblOrder == null)
            {
                return HttpNotFound();
            }
            return View(tblOrder);
        }

        // POST: Admin/Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblOrder tblOrder = db.tblOrders.Find(id);
            db.tblOrders.Remove(tblOrder);
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
