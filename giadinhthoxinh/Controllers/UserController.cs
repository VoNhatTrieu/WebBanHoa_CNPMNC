using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using giadinhthoxinh.Models;

namespace giadinhthoxinh.Controllers
{
    public class UserController : Controller
    {
        giadinhthoxinhEntities1 db = new giadinhthoxinhEntities1();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        //Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string sEmail, string sPass)
        {
            // Validate input - comprehensive checks
            if (string.IsNullOrWhiteSpace(sEmail) || string.IsNullOrWhiteSpace(sPass))
            {
                TempData["Error"] = "❌ Vui lòng nhập đầy đủ email và mật khẩu!";
                return View();
            }

            // Check for dangerous characters in email
            if (ContainsDangerousCharacters(sEmail))
            {
                TempData["Error"] = "❌ Email chứa ký tự không hợp lệ!";
                return View();
            }

            // Check email format
            if (!IsValidEmail(sEmail))
            {
                TempData["Error"] = "❌ Email không đúng định dạng!";
                return View();
            }

            // Check password length
            if (sPass.Length < 6 || sPass.Length > 100)
            {
                TempData["Error"] = "❌ Mật khẩu phải có độ dài từ 6-100 ký tự!";
                return View();
            }

            // Check for dangerous characters in password
            if (ContainsDangerousPasswordCharacters(sPass))
            {
                TempData["Error"] = "❌ Mật khẩu chứa ký tự nguy hiểm!";
                return View();
            }

            var f_password = GetMD5(sPass);
            var ktraadmin = db.tblUsers.Where(s => s.sEmail.Equals(sEmail) && s.sPass.Equals(f_password)).ToList();
            if (ktraadmin.Count>0 && ktraadmin[0].FK_iPermissionID>1)
            {
                Session["Admin"] = ktraadmin[0];
                var tmp = (tblUser)Session["Admin"];
                if (tmp.FK_iPermissionID == 2)// id quyen bang 2 thi chi co quyen nhan vien
                {
                    Session["Nhanvien"] = Session["Admin"];
                }
                if (tmp.FK_iPermissionID == 3)// id quyen bang 2 thi co quyen admin
                {
                    Session["Nhanvien"] = Session["Admin"];
                    Session["QuanLy"] = Session["Admin"];
                }
                TempData["Success"] = "✅ Đăng nhập thành công! Chào mừng quản trị viên.";
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
            else if (ModelState.IsValid)
            {
                
                var data = db.tblUsers.Where(s => s.sEmail.Equals(sEmail) && s.sPass.Equals(f_password)).ToList();
                if (data.Count() > 0)
                {
                    //add session
                    Session["idUser"] = data.FirstOrDefault().PK_iAccountID;
                    Session["userName"] = data.FirstOrDefault().sUserName;
                    Session["Email"] = data.FirstOrDefault().sEmail;
                    Session["User"] = data[0];
                    TempData["Success"] = "✅ Đăng nhập thành công! Chào mừng bạn trở lại.";
                    return RedirectToAction("index", "home");
                }
                else
                {
                    TempData["Error"] = "❌ Email hoặc mật khẩu không chính xác!";
                    return View();
                }
            }

            TempData["Error"] = "❌ Có lỗi xảy ra. Vui lòng thử lại!";
            return View();
        }
        public ActionResult AccountPartial()
        {
            
            if (Session["User"]!=null)
            {

                var kh = (tblUser)Session["User"];
                var thongtinkhachhang = db.tblUsers.Find(kh.PK_iAccountID);
                ViewBag.TenKH = thongtinkhachhang.sUserName.ToString();
            }


            return PartialView();
        }
        //Register    
        public ActionResult Register()
        {
            ViewBag.FK_iPermissionID = new SelectList(db.tblPermissions, "PK_iPermissionID", "sPermissionName");
            return View();
        }

        //public ActionResult Register1()
        //{
        //    return View();
        //}

        //POST: Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(tblUser _user)
        {
            if (ModelState.IsValid)
            {
                // Comprehensive validation for email
                if (string.IsNullOrWhiteSpace(_user.sEmail))
                {
                    TempData["Error"] = "❌ Vui lòng nhập email!";
                    return View(_user);
                }

                if (_user.sEmail.Length < 5 || _user.sEmail.Length > 100)
                {
                    TempData["Error"] = "❌ Email phải có độ dài từ 5-100 ký tự!";
                    return View(_user);
                }

                if (ContainsDangerousCharacters(_user.sEmail))
                {
                    TempData["Error"] = "❌ Email chứa ký tự không hợp lệ!";
                    return View(_user);
                }

                if (!IsValidEmail(_user.sEmail))
                {
                    TempData["Error"] = "❌ Email không đúng định dạng!";
                    return View(_user);
                }

                // Validate password
                if (string.IsNullOrWhiteSpace(_user.sPass))
                {
                    TempData["Error"] = "❌ Vui lòng nhập mật khẩu!";
                    return View(_user);
                }

                if (_user.sPass.Length < 6 || _user.sPass.Length > 100)
                {
                    TempData["Error"] = "❌ Mật khẩu phải có độ dài từ 6-100 ký tự!";
                    return View(_user);
                }

                if (ContainsDangerousPasswordCharacters(_user.sPass))
                {
                    TempData["Error"] = "❌ Mật khẩu chứa ký tự nguy hiểm!";
                    return View(_user);
                }

                // Validate username
                if (string.IsNullOrWhiteSpace(_user.sUserName))
                {
                    TempData["Error"] = "❌ Vui lòng nhập họ và tên!";
                    return View(_user);
                }

                if (_user.sUserName.Length < 2 || _user.sUserName.Length > 50)
                {
                    TempData["Error"] = "❌ Họ tên phải có độ dài từ 2-50 ký tự!";
                    return View(_user);
                }

                if (ContainsDangerousCharacters(_user.sUserName))
                {
                    TempData["Error"] = "❌ Họ tên chứa ký tự không hợp lệ!";
                    return View(_user);
                }

                // Validate phone
                if (string.IsNullOrWhiteSpace(_user.sPhone))
                {
                    TempData["Error"] = "❌ Vui lòng nhập số điện thoại!";
                    return View(_user);
                }

                string phoneDigitsOnly = _user.sPhone.Replace("+", "");
                if (!System.Text.RegularExpressions.Regex.IsMatch(phoneDigitsOnly, @"^\d+$"))
                {
                    TempData["Error"] = "❌ Số điện thoại chỉ được chứa số!";
                    return View(_user);
                }

                if (phoneDigitsOnly.Length < 10 || phoneDigitsOnly.Length > 15)
                {
                    TempData["Error"] = "❌ Số điện thoại phải có từ 10-15 chữ số!";
                    return View(_user);
                }

                // Validate address
                if (string.IsNullOrWhiteSpace(_user.sAddress))
                {
                    TempData["Error"] = "❌ Vui lòng nhập địa chỉ!";
                    return View(_user);
                }

                if (_user.sAddress.Length < 10 || _user.sAddress.Length > 200)
                {
                    TempData["Error"] = "❌ Địa chỉ phải có độ dài từ 10-200 ký tự!";
                    return View(_user);
                }

                if (ContainsDangerousCharacters(_user.sAddress))
                {
                    TempData["Error"] = "❌ Địa chỉ chứa ký tự không hợp lệ!";
                    return View(_user);
                }

                // Check if email already exists
                var check = db.tblUsers.FirstOrDefault(s => s.sEmail == _user.sEmail);
                if (check == null)
                {
                    _user.sPass = GetMD5(_user.sPass);
                    _user.FK_iPermissionID = 1; // Set default permission to customer
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.tblUsers.Add(_user);
                    db.SaveChanges();
                    
                    TempData["Success"] = "✅ Đăng ký thành công! Vui lòng đăng nhập.";
                    return RedirectToAction("Login");
                }
                else
                {
                    TempData["Error"] = "❌ Email này đã được đăng ký. Vui lòng sử dụng email khác!";
                    return View(_user);
                }
            }
            
            TempData["Error"] = "❌ Vui lòng điền đầy đủ thông tin hợp lệ!";
            return View(_user);
        }

        //create a string MD5
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }

        // Helper method to check for dangerous characters
        private bool ContainsDangerousCharacters(string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            // Check for common XSS and SQL injection characters
            char[] dangerousChars = { '<', '>', ';', '"', '\'', '`', '\\' };
            return input.IndexOfAny(dangerousChars) >= 0;
        }

        // Helper method to check for dangerous password characters
        private bool ContainsDangerousPasswordCharacters(string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            // More restrictive for passwords
            char[] dangerousChars = { '<', '>', ';', '"', '\'', '`' };
            return input.IndexOfAny(dangerousChars) >= 0;
        }

        // Helper method to validate email format
        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Check for basic email pattern
                var regex = new System.Text.RegularExpressions.Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                if (!regex.IsMatch(email))
                    return false;

                // Check for spaces
                if (email.Contains(" "))
                    return false;

                // Check length
                if (email.Length < 5 || email.Length > 100)
                    return false;

                return true;
            }
            catch
            {
                return false;
            }
        }

        public ActionResult Permission()
        {
            return View();
        }
        public ActionResult Edit()
        {
           
            if (Session["User"] != null)
            {
                var nguoidung = (tblUser)Session["User"];
                var nguoidung_sua = db.tblUsers.Find(nguoidung.PK_iAccountID);
                return View(nguoidung_sua);
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
                
           
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(tblUser _user)
        {
            if (ModelState.IsValid)
            {
                var olddata = db.tblUsers.FirstOrDefault(s => s.PK_iAccountID == _user.PK_iAccountID);
                if (olddata != null)
                {
                    olddata.sUserName = _user.sUserName;
                    olddata.sEmail = _user.sEmail;
                    olddata.sPhone = _user.sPhone;
                    olddata.sAddress = _user.sAddress;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.error = "Không tồn tại tài khoản";
                    return RedirectToAction("Login", "User"); ;
                }


            }
            //ViewBag.FK_iPermissionID = new SelectList(db.tblPermissions, "PK_iPermissionID", "sPermissionName", tblUser.FK_iPermissionID);
            return View();
        }
        public ActionResult EditPassword()
        {

            if (Session["User"] != null)
            {
               
                return View();
            }
            else
            {
                return RedirectToAction("Login", "User");
            }


        }
        [HttpPost]
        public ActionResult EditPassword(tblUser _user)
        {
            if (ModelState.IsValid)
            {

                var nguoidung = (tblUser)Session["User"];
                var nguoidung_sua = db.tblUsers.Find(nguoidung.PK_iAccountID);
                var input = nguoidung_sua.sPass;
                var label = GetMD5(Request.Form["oldpass"].ToString());
                if ( input==label)
                {
                    nguoidung_sua.sPass = GetMD5(Request.Form["newpass"]);
                    db.SaveChanges();
                    ViewBag.error = "Đổi mật khẩu thành công!";
                    
                }
                else
                {
                    ViewBag.error = "Sai mật khẩu!";
                }
                    

            }
            //ViewBag.FK_iPermissionID = new SelectList(db.tblPermissions, "PK_iPermissionID", "sPermissionName", tblUser.FK_iPermissionID);
            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear();//remove session
            return RedirectToAction("index", "home");

        }

    }
}