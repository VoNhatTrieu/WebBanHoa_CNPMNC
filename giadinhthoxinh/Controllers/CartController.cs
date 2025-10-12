using giadinhthoxinh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace giadinhthoxinh.Controllers
{
    public class CartController : Controller
    {
        private static giadinhthoxinhEntities1 db = new giadinhthoxinhEntities1();
        private  List<tblProduct> listProduct = db.tblProducts.ToList();
        // GET: Cart
        public List<ProductInCart> LayGioHang()
        {
            Cart lstGioHang = Session["GioHang"] as Cart;
            if (lstGioHang == null)
            {
                //Nếu giỏ hàng chưa tồn tại thì mình tiến hành khởi tao list giỏ hàng (sessionGioHang)
                lstGioHang = new Cart();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang.lstproduct;
        }
        [HttpPost]
        public ActionResult ThemGioHang(int iMasp, string strURL)
        {
            int soluong = int.Parse(Request.Form["quantity_value"]);
            tblProduct sp = db.tblProducts.SingleOrDefault(n => n.PK_iProductID == iMasp);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Lấy ra session giỏ hàng
            List<ProductInCart> giohang = LayGioHang();
            //Kiểm tra sp này đã tồn tại trong session[giohang] chưa
            ProductInCart sanpham = giohang.Find(n => n.ProductID == iMasp); //lstGioHang.Find(n => n.PK_iProductID == iMasp);
            if (sanpham == null)
            {
              
                sanpham = new ProductInCart(iMasp,soluong);
                //Add sản phẩm mới thêm vào list
                giohang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.Quatity++;
                return Redirect(strURL);
            }
        }
        public ActionResult XoaGioHang(int iMaSP)
        {
            //Kiểm tra masp
            tblProduct sp = db.tblProducts.SingleOrDefault(n => n.PK_iProductID == iMaSP);
            //Nếu get sai masp thì sẽ trả về trang lỗi 404
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Lấy giỏ hàng ra từ session
            List<ProductInCart> lstGioHang = LayGioHang();
            ProductInCart sanpham = lstGioHang.SingleOrDefault(n => n.ProductID == iMaSP);
            //Nếu mà tồn tại thì chúng ta cho sửa số lượng
            if (sanpham != null)
            {
                lstGioHang.RemoveAll(n => n.ProductID == iMaSP);

            }
            if (lstGioHang.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Cart");
        }
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<ProductInCart> lstGioHang = LayGioHang();
            if (lstGioHang != null)
            {
                iTongSoLuong = lstGioHang.Sum(n => n.Quatity);
            }
            return iTongSoLuong;
        }
        //Tính tổng thành tiền
        private double TongTien()
        {
            double dTongTien = 0;
            List<ProductInCart> lstGioHang = LayGioHang();
            if (lstGioHang != null)
            {
                dTongTien = lstGioHang.Sum(n => n.ThanhTien);
            }
            return dTongTien;
        }
        public ActionResult Cart()
        {
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<ProductInCart> lstGioHang = LayGioHang();
            ViewBag.soluongsp = TongSoLuong();
            ViewBag.tongtien = TongTien();
            return View(lstGioHang);
        }
        public ActionResult CartPartial()
        {
            if (TongSoLuong() == 0)
            {
                ViewBag.TongSoLuong = 0;
                return PartialView();
            }
            ViewBag.TongSoLuong = TongSoLuong();          
            return PartialView();
        }
        public ActionResult Checkout()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            else
            {
                List<ProductInCart> lstGioHang = LayGioHang();
                int sum = 0;
  
                ViewBag.TongTien = TongTien();
                ViewBag.SanPham = lstGioHang;
                ViewBag.NguoiNhan = (tblUser)Session["User"];
                return View();
            }
                
                
            return View();
        }
        /*
         * Thanh toán 
         * giohang.lstproduct.Clear();
         */
        [HttpPost]
    
        public ActionResult DatHang()
        {
            //Kiểm tra đăng đăng nhập
            if (Session["User"] == null || Session["User"].ToString() == "")
            {
                return RedirectToAction("Login", "User");
            }
            //Kiểm tra giỏ hàng
            if (Session["User"] == null)
            {
                RedirectToAction("Index", "Home");
            }
           
            //Thêm đơn hàng
            tblOrder ddh = new tblOrder();
            List<ProductInCart> gh = LayGioHang();
          
            tblUser kh = (tblUser)Session["User"];
            //ddh.ma_DH = SinhMaDH();
            ddh.FK_iAccountID = kh.PK_iAccountID;
            ddh.sDeliveryAddress = Request.Form["sDeliveryAddress"];
            ddh.sCustomerName = Request.Form["sCustomerName"];
            ddh.sCustomerPhone = Request.Form["sCustomerPhone"];
            ddh.iDeliveryMethod= int.Parse(Request.Form["iDeliveryMethod"]);
            ddh.iPaid = 0;
            ddh.dInvoidDate = DateTime.Now;
            ddh.fSurcharge= float.Parse(TongTien().ToString());          
            ddh.sState = "Chờ xác nhận";
            int sum = 0;
            Console.WriteLine(ddh);
            db.tblOrders.Add(ddh);
            db.SaveChanges();
            //Thêm chi tiết đơn hàng
            foreach (var item in gh)
            {
                tblCheckoutDetail ctDH = new tblCheckoutDetail();
                ctDH.FK_iOrderID = ddh.PK_iOrderID;
                ctDH.FK_iProductID = item.ProductID;
                ctDH.iQuantity = item.Quatity;
                ctDH.fPrice = item.Price;
                db.tblCheckoutDetails.Add(ctDH);
                // db.Chitietdonhangs.Add(ctDH);
            }
            db.SaveChanges();
            Session["GioHang"] = null;
            return RedirectToAction("OrderDetail", new { id = ddh.PK_iOrderID });
        
        }

        // Chi tiết đơn hàng
        public ActionResult OrderDetail(int id)
        {
            // Lấy thông tin đơn hàng theo ID
            var order = db.tblOrders.FirstOrDefault(o => o.PK_iOrderID == id);
            
            if (order == null)
            {
                return RedirectToAction("Index", "Home");
            }

            // Lấy chi tiết đơn hàng
            var orderDetails = db.tblCheckoutDetails
                .Where(od => od.FK_iOrderID == id)
                .ToList();

            // Truyền dữ liệu sang View
            ViewBag.Order = order;
            ViewBag.OrderDetails = orderDetails;
            ViewBag.Customer = order.tblUser;
            
            // Tính tổng tiền
            double totalAmount = 0;
            if (orderDetails != null && orderDetails.Count > 0)
            {
                foreach (var item in orderDetails)
                {
                    if (item.fPrice.HasValue && item.iQuantity.HasValue)
                    {
                        totalAmount += item.fPrice.Value * item.iQuantity.Value;
                    }
                }
            }
            ViewBag.TotalAmount = totalAmount;

            return View();
        }

        private double getProductPrice(int productid)
        {
            double price = 0;
            foreach(var item in listProduct)
            {
                if (item.PK_iProductID == productid)
                {
                    price = (double)item.fPrice;
                    break;
                }
            }
            return price;
        }
    }
}