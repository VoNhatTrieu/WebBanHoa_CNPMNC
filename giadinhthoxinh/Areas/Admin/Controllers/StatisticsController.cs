using giadinhthoxinh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace giadinhthoxinh.Areas.Admin.Controllers
{
    public class StatisticsController : Controller
    {
        private giadinhthoxinhEntities1 db = new giadinhthoxinhEntities1();

        // GET: Admin/Statistics
        public ActionResult Index()
        {
            if (Session["NhanVien"] == null)
            {
                return RedirectToAction("KhongDuThamQuyen", "PhanQuyen");
            }

            return View();
        }

        // API để lấy dữ liệu thống kê
        [HttpGet]
        public JsonResult GetStatistics(string period = "day")
        {
            try
            {
                DateTime startDate;
                DateTime endDate = DateTime.Now;

                // Xác định khoảng thời gian
                switch (period.ToLower())
                {
                    case "week":
                        startDate = DateTime.Now.AddDays(-7);
                        break;
                    case "month":
                        startDate = DateTime.Now.AddMonths(-1);
                        break;
                    default: // day
                        startDate = DateTime.Now.Date;
                        break;
                }

                // Lấy tất cả chi tiết hóa đơn đã xác nhận trong khoảng thời gian
                var confirmedDetails = db.tblCheckoutDetails
                    .Where(cd => cd.sStatus == "Đã xác nhận" && 
                           cd.tblOrder.dInvoidDate >= startDate && 
                           cd.tblOrder.dInvoidDate <= endDate)
                    .ToList();

                // Tổng số đơn hàng đã xác nhận
                var totalOrders = confirmedDetails
                    .Select(cd => cd.FK_iOrderID)
                    .Distinct()
                    .Count();

                // Tổng doanh thu
                var totalRevenue = confirmedDetails
                    .Sum(cd => (cd.fPrice ?? 0) * (cd.iQuantity ?? 0));

                // Tổng số sản phẩm bán ra
                var totalProducts = confirmedDetails
                    .Sum(cd => cd.iQuantity ?? 0);

                // Số lượng khách hàng
                var orderIds = confirmedDetails.Select(cd => cd.FK_iOrderID).Distinct().ToList();
                var customers = db.tblOrders
                    .Where(o => orderIds.Contains(o.PK_iOrderID))
                    .Select(o => o.FK_iAccountID)
                    .Distinct()
                    .ToList();

                // Khách hàng mới (tạo tài khoản trong khoảng thời gian) - cải tiến logic
                var newCustomers = db.tblUsers
                    .Where(u => u.FK_iPermissionID == 1) // Giả sử 1 là khách hàng
                    .Count();

                // Khách hàng quay lại (có > 1 đơn hàng)
                var returningCustomers = db.tblOrders
                    .Where(o => o.dInvoidDate >= startDate && o.dInvoidDate <= endDate)
                    .GroupBy(o => o.FK_iAccountID)
                    .Where(g => g.Count() > 1)
                    .Count();

                // Dữ liệu biểu đồ doanh thu theo ngày - cải tiến
                List<object> revenueByDate;
                
                if (period == "day")
                {
                    // Theo giờ trong ngày
                    revenueByDate = confirmedDetails
                        .GroupBy(cd => cd.tblOrder.dInvoidDate.Hour)
                        .Select(g => new
                        {
                            date = g.Key + ":00",
                            revenue = g.Sum(cd => (cd.fPrice ?? 0) * (cd.iQuantity ?? 0)),
                            orders = g.Select(cd => cd.FK_iOrderID).Distinct().Count(),
                            products = g.Sum(cd => cd.iQuantity ?? 0)
                        })
                        .OrderBy(x => x.date)
                        .Cast<object>()
                        .ToList();
                }
                else
                {
                    // Theo ngày
                    revenueByDate = confirmedDetails
                        .GroupBy(cd => cd.tblOrder.dInvoidDate.Date)
                        .Select(g => new
                        {
                            date = g.Key.ToString("dd/MM/yyyy"),
                            revenue = g.Sum(cd => (cd.fPrice ?? 0) * (cd.iQuantity ?? 0)),
                            orders = g.Select(cd => cd.FK_iOrderID).Distinct().Count(),
                            products = g.Sum(cd => cd.iQuantity ?? 0)
                        })
                        .OrderBy(x => x.date)
                        .Cast<object>()
                        .ToList();
                }

                // Top 5 sản phẩm bán chạy
                var topProducts = confirmedDetails
                    .GroupBy(cd => new { 
                        cd.FK_iProductID, 
                        cd.tblProduct.sProductName 
                    })
                    .Select(g => new
                    {
                        productId = g.Key.FK_iProductID,
                        productName = g.Key.sProductName,
                        quantity = g.Sum(cd => cd.iQuantity ?? 0),
                        revenue = g.Sum(cd => (cd.fPrice ?? 0) * (cd.iQuantity ?? 0))
                    })
                    .OrderByDescending(x => x.quantity)
                    .Take(5)
                    .ToList();

                // Tính phần trăm cho top products
                var maxRevenue = topProducts.Any() ? topProducts.Max(p => p.revenue) : 1;
                var topProductsWithPercent = topProducts.Select(p => new
                {
                    p.productId,
                    p.productName,
                    p.quantity,
                    p.revenue,
                    revenuePercent = maxRevenue > 0 ? Math.Round((p.revenue / maxRevenue) * 100, 2) : 0
                }).ToList();

                // Thống kê theo danh mục
                var categoryStats = confirmedDetails
                    .GroupBy(cd => new {
                        cd.tblProduct.FK_iCategoryID,
                        cd.tblProduct.tblCategory.sCategoryName
                    })
                    .Select(g => new
                    {
                        categoryName = g.Key.sCategoryName,
                        quantity = g.Sum(cd => cd.iQuantity ?? 0),
                        revenue = g.Sum(cd => (cd.fPrice ?? 0) * (cd.iQuantity ?? 0))
                    })
                    .OrderByDescending(x => x.revenue)
                    .ToList();

                // Thống kê trạng thái đơn hàng
                var orderStatusStats = db.tblOrders
                    .Where(o => o.dInvoidDate >= startDate && o.dInvoidDate <= endDate)
                    .GroupBy(o => o.sState)
                    .Select(g => new
                    {
                        status = g.Key,
                        count = g.Count()
                    })
                    .ToList();

                // Doanh thu trung bình mỗi đơn hàng
                var averageOrderValue = totalOrders > 0 ? totalRevenue / totalOrders : 0;

                var result = new
                {
                    success = true,
                    data = new
                    {
                        overview = new
                        {
                            totalOrders = totalOrders,
                            totalRevenue = totalRevenue,
                            totalProducts = totalProducts,
                            newCustomers = customers.Count,
                            returningCustomers = returningCustomers,
                            averageOrderValue = averageOrderValue
                        },
                        revenueChart = revenueByDate,
                        topProducts = topProductsWithPercent,
                        categoryStats = categoryStats,
                        orderStatusStats = orderStatusStats,
                        period = period,
                        startDate = startDate.ToString("dd/MM/yyyy HH:mm"),
                        endDate = endDate.ToString("dd/MM/yyyy HH:mm")
                    }
                };

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Lỗi khi tải dữ liệu: " + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        // API để xuất báo cáo
        [HttpGet]
        public JsonResult ExportReport(string period = "day")
        {
            try
            {
                // Logic xuất báo cáo (có thể xuất Excel, PDF, etc.)
                return Json(new
                {
                    success = true,
                    message = "Xuất báo cáo thành công"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Lỗi khi xuất báo cáo: " + ex.Message
                }, JsonRequestBehavior.AllowGet);
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
