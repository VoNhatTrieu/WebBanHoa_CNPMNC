using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using giadinhthoxinh.Helpers;
using giadinhthoxinh.Models;
using System.Configuration;

namespace giadinhthoxinh.Controllers
{
    public class VnPayController : Controller
    {
        private static giadinhthoxinhEntities1 db = new giadinhthoxinhEntities1();

        public ActionResult Pay(int orderId, int amount)
        {
            string vnp_Returnurl = ConfigurationManager.AppSettings["vnp_Returnurl"];
            string vnp_Url = ConfigurationManager.AppSettings["vnp_Url"];
            string vnp_TmnCode = ConfigurationManager.AppSettings["vnp_TmnCode"];
            string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"];

            var vnPay = new VnPayLibrary();

            vnPay.AddRequestData("vnp_Version", "2.1.0");
            vnPay.AddRequestData("vnp_Command", "pay");
            vnPay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnPay.AddRequestData("vnp_Amount", (amount * 100).ToString());
            vnPay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            vnPay.AddRequestData("vnp_CurrCode", "VND");
            vnPay.AddRequestData("vnp_IpAddr", Request.UserHostAddress);
            vnPay.AddRequestData("vnp_Locale", "vn");
            vnPay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang: " + orderId);
            vnPay.AddRequestData("vnp_OrderType", "other");
            vnPay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);

            string txnRef = DateTime.Now.Ticks.ToString();
            vnPay.AddRequestData("vnp_TxnRef", txnRef);

            vnPay.AddRequestData("vnp_ExpireDate", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));

            string paymentUrl = vnPay.CreateRequestUrl(vnp_Url, vnp_HashSecret);

            return Redirect(paymentUrl);
        }

        public ActionResult Return()
        {
            var vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"];
            var vnPay = new VnPayLibrary();
            var queryParams = Request.QueryString;

            foreach (string key in queryParams.AllKeys)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnPay.AddResponseData(key, queryParams[key]);
                }
            }

            string vnp_SecureHash = queryParams["vnp_SecureHash"];
            var checkSignature = vnPay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);

            var response = new VnPaymentResponseModel();
            response.Success = false;

            if (checkSignature)
            {
                string responseCode = vnPay.GetResponseData("vnp_ResponseCode");
                response.OrderId = vnPay.GetResponseData("vnp_TxnRef");
                response.TransactionId = vnPay.GetResponseData("vnp_TransactionNo");
                response.VnPayResponseCode = responseCode;
                response.Success = responseCode == "00";

                // ✅ NẾU THANH TOÁN VNPAY THÀNH CÔNG - CẬP NHẬT TRẠNG THÁI ĐƠN HÀNG
                if (response.Success)
                {
                    try
                    {
                        // Tìm đơn hàng theo OrderId (từ vnp_TxnRef)
                        int orderId = int.Parse(response.OrderId.Split(':').Last());
                        var order = db.tblOrders.Find(orderId);

                        if (order != null)
                        {
                            // ✅ Cập nhật trạng thái: thanh toán VNPay thành công → "Đã xác nhận"
                            order.sState = "Đã xác nhận";
                            order.iPaid = 1;  // ✅ Đã thanh toán qua VNPay
                            order.sBiller = "VNPay";  // ✅ Ghi phương thức thanh toán

                            // Cập nhật chi tiết đơn hàng
                            var orderDetails = db.tblCheckoutDetails.Where(x => x.FK_iOrderID == orderId);
                            foreach (var item in orderDetails)
                            {
                                item.sStatus = "Đã xác nhận";
                            }

                            db.SaveChanges();
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log error nếu cần
                    }
                }
            }

            return View(response);
        }
    }
}