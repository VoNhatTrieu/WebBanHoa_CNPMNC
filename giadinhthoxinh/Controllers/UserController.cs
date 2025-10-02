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
                    return RedirectToAction("index", "home");
                }
                else
                {
                    ViewBag.error = "Login failed";
                    return RedirectToAction("login");
                }
            }

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
                var check = db.tblUsers.FirstOrDefault(s => s.sEmail == _user.sEmail);
                if (check == null)
                {
                    _user.sPass = GetMD5(_user.sPass);
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.tblUsers.Add(_user);
                    db.SaveChanges();
                    return RedirectToAction("index", "home");
                }
                else
                {
                    ViewBag.error = "Email này đã tồn tại";
                    return View();
                }


            }
            //ViewBag.FK_iPermissionID = new SelectList(db.tblPermissions, "PK_iPermissionID", "sPermissionName", tblUser.FK_iPermissionID);
            return View();
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