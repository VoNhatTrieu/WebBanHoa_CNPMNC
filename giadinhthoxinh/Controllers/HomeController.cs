using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using giadinhthoxinh.Models;
using PagedList;

namespace giadinhthoxinh.Controllers
{
    public class HomeController : Controller
    {
        public giadinhthoxinhEntities1 db = new giadinhthoxinhEntities1();
        public ActionResult Index(int? page)
        {
            
           
            int productInPage = 10;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            giadinhthoxinhEntities1 db = new giadinhthoxinhEntities1();

            List<tblProduct> ketQua = db.tblProducts.ToList();
          
            IPagedList<tblProduct> ketQuaFinal = null;
            ketQuaFinal = ketQua.ToPagedList(pageNumber, productInPage);
           // var ketQua = db.tblProducts.ToList();
           //  PagedList<tblProduct> ketQuaFinal = new PagedList<tblProduct>(ketQua, pageNumber, productInPage);
            return View(ketQuaFinal);
        }
        public ActionResult Search(string searchString, int? page)
        {
            Session["Search"] = searchString;
            int productInPage = 10;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            giadinhthoxinhEntities1 db = new giadinhthoxinhEntities1();

            List<tblProduct> ketQua = db.tblProducts.ToList();
            IOrderedQueryable<tblProduct> model = (IOrderedQueryable<tblProduct>)db.tblProducts.OrderByDescending(x => x.PK_iProductID);
            if (!String.IsNullOrEmpty(searchString))
            {
                model = (IOrderedQueryable<tblProduct>)model.Where(x => x.sProductName.Contains(searchString));
                IPagedList<tblProduct> timkiem = null;
                timkiem = model.ToPagedList(pageNumber, productInPage);
                return View(timkiem);
            }
            IPagedList<tblProduct> ketQuaFinal = null;
            ketQuaFinal = ketQua.ToPagedList(pageNumber, productInPage);
            // var ketQua = db.tblProducts.ToList();
            //  PagedList<tblProduct> ketQuaFinal = new PagedList<tblProduct>(ketQua, pageNumber, productInPage);
            return View(ketQuaFinal);
        }
        // Sản phẩm theo danh mục
        public ActionResult SanPhamTheoDanhMuc(int id, int? page)
        {
            int pageSize = 10, pageNumber = page.GetValueOrDefault(1);
            var q = db.tblProducts
                      .Where(p => p.FK_iCategoryID == id)
                      .OrderByDescending(p => p.PK_iProductID);

            ViewBag.ActiveCategoryId = id;            // để highlight trong menu
            return View("Index", q.ToPagedList(pageNumber, pageSize));
        }

        // Dropdown danh mục (chỉ danh mục có chữ "Hoa")
        public PartialViewResult DanhMucPartial(int? activeId)   // nhận id hiện hành
        {
            var cats = db.tblCategories
                         .Where(c => c.sCategoryName.Contains("Hoa"))
                         .OrderBy(c => c.sCategoryName)
                         .ToList();
            ViewBag.ActiveCategoryId = activeId;
            return PartialView("~/Views/Shared/DanhMucPartial.cshtml", cats);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult ProductDetail(int id)
        {
            var item = db.tblProducts.Find(id);

            return View(item);
        }
       // [HttpPost]
       // [ValidateAntiForgeryToken]
      /*  public ActionResult ProductDetail(int product_id, int quantity_value)
        {
            //xu ly them vao gio
                Cart giohang = (Cart)Session["giohang"];
                ProductInCart sanpham = new ProductInCart();
                sanpham.ProductID = product_id;
                sanpham.Quatity = quantity_value;
                int check = 0;
                if (giohang.lstproduct != null && giohang.lstproduct.Count > 0)
                    foreach (ProductInCart a in giohang.lstproduct)
                    {
                        if (a.ProductID == sanpham.ProductID)
                        {
                            a.Quatity++;
                            check = 1;
                            break;
                        }
                    }
                if (check == 0)
                {
                    giohang.lstproduct.Add(sanpham);
                }


            return RedirectToAction("ProductDetail", new { id = product_id });
        //    var item = db.tblProducts.Find(product_id);
         //   return View(item);
        }*/
        public ActionResult Checkout()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Promote()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
    }
}