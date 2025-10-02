using giadinhthoxinh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace giadinhthoxinh.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            if (Session["NhanVien"] == null)
            {
                return RedirectToAction("KhongDuThamQuyen", "PhanQuyen");
            }
            else
            {
              
                return View();
            }
                
            
        }
      
    }
}