using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace giadinhthoxinh.Areas.Admin.Controllers
{
    public class PhanquyenController : Controller
    {
        // GET: Admin/Phanquyen
        public ActionResult KhongDuThamQuyen()
        {
            return View();
        }
    }
}