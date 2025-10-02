using giadinhthoxinh.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace giadinhthoxinh.Areas.Admin.Controllers
{
    public class ReportController : Controller
    {
        string str = @"Data Source=LAPTOP-OLF1LQUT;Initial Catalog=giadinhthoxinh;Integrated Security=True";
        SqlConnection connection;
        SqlCommand command;
        SqlDataAdapter adapter = new SqlDataAdapter();
        // GET: Admin/Report
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult TKNhapHang()
        {
            if (Session["QuanLy"] != null)
            {

                DataTable table1 = new DataTable();
                connection = new SqlConnection(str);
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = "select tblCheckinDetail.FK_iProductID,tblProduct.sProductName,tblProduct.sDescribe,tblCheckinDetail.fPrice,sum(tblCheckinDetail.iQuatity)as'soluong' from tblProduct, tblCheckinDetail where tblProduct.PK_iProductID = tblCheckinDetail.FK_iProductID group by tblCheckinDetail.FK_iProductID,tblProduct.sProductName,tblProduct.sDescribe,tblCheckinDetail.fPrice";
                adapter.SelectCommand = command;
                table1.Clear();
                adapter.Fill(table1);
                List<NhapHang> ds = new List<NhapHang>();
                for (int i = 0; i < table1.Rows.Count; i++)
                {
                    NhapHang kq = new NhapHang();

                    kq.ID = table1.Rows[i]["FK_iProductID"].ToString();
                    kq.Name= table1.Rows[i]["sProductName"].ToString();
                    kq.Description = table1.Rows[i]["sDescribe"].ToString();
                    kq.Price = table1.Rows[i]["fPrice"].ToString();
                    kq.Quatity= table1.Rows[i]["soluong"].ToString();
                    ds.Add(kq);
                }
                ViewBag.MyList = ds;

                return View();

            }
            else
            {
                return RedirectToAction("KhongDuThamQuyen", "PhanQuyen");
            }

        }
        public ActionResult TKBanHang()
        {
            if (Session["QuanLy"] != null)
            {

                DataTable table1 = new DataTable();
                connection = new SqlConnection(str);
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = "select tblCheckoutDetail.FK_iProductID,tblProduct.sProductName,tblProduct.sDescribe,tblCheckoutDetail.fPrice,sum(tblCheckoutDetail.iQuantity)as'soluong' from tblProduct, tblCheckoutDetail where tblProduct.PK_iProductID = tblCheckoutDetail.FK_iProductID group by tblCheckoutDetail.FK_iProductID,tblProduct.sProductName,tblProduct.sDescribe,tblCheckoutDetail.fPrice";
                adapter.SelectCommand = command;
                table1.Clear();
                adapter.Fill(table1);
                List<NhapHang> ds = new List<NhapHang>();
                for (int i = 0; i < table1.Rows.Count; i++)
                {
                    NhapHang kq = new NhapHang();

                    kq.ID = table1.Rows[i]["FK_iProductID"].ToString();
                    kq.Name = table1.Rows[i]["sProductName"].ToString();
                    kq.Description = table1.Rows[i]["sDescribe"].ToString();
                    kq.Price = table1.Rows[i]["fPrice"].ToString();
                    kq.Quatity = table1.Rows[i]["soluong"].ToString();
                    ds.Add(kq);
                }
                ViewBag.MyList = ds;

                return View();

            }
            else
            {
                return RedirectToAction("KhongDuThamQuyen", "PhanQuyen");
            }

        }
        public ActionResult SanPhamBanChay()
        {
            if (Session["QuanLy"] != null)
            {

                DataTable table1 = new DataTable();
                connection = new SqlConnection(str);
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = "select top (5) tblCheckoutDetail.FK_iProductID,tblProduct.sProductName,tblProduct.sDescribe,tblCheckoutDetail.fPrice,sum(tblCheckoutDetail.iQuantity)as'soluong' from tblProduct, tblCheckoutDetail where tblProduct.PK_iProductID = tblCheckoutDetail.FK_iProductID group by tblCheckoutDetail.FK_iProductID,tblProduct.sProductName,tblProduct.sDescribe,tblCheckoutDetail.fPrice order by 'soluong' desc";
                adapter.SelectCommand = command;
                table1.Clear();
                adapter.Fill(table1);
                List<NhapHang> ds = new List<NhapHang>();
                for (int i = 0; i < table1.Rows.Count; i++)
                {
                    NhapHang kq = new NhapHang();

                    kq.ID = table1.Rows[i]["FK_iProductID"].ToString();
                    kq.Name = table1.Rows[i]["sProductName"].ToString();
                    kq.Description = table1.Rows[i]["sDescribe"].ToString();
                    kq.Price = table1.Rows[i]["fPrice"].ToString();
                    kq.Quatity = table1.Rows[i]["soluong"].ToString();
                    ds.Add(kq);
                }
                ViewBag.MyList = ds;

                return View();

            }
            else
            {
                return RedirectToAction("KhongDuThamQuyen", "PhanQuyen");
            }
        }
        public ActionResult TKLoaiSanPham()
        {
            if (Session["QuanLy"] != null)
            {

                DataTable table1 = new DataTable();
                connection = new SqlConnection(str);
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = "  select tblCategory.PK_iCategoryID,tblCategory.sCategoryName,count(tblProduct.PK_iProductID)as'soluong' from tblCategory, tblProduct where tblCategory.PK_iCategoryID = tblProduct.FK_iCategoryID group by tblCategory.PK_iCategoryID,tblCategory.sCategoryName";
                adapter.SelectCommand = command;
                table1.Clear();
                adapter.Fill(table1);
                List<TkLoaiSp> ds = new List<TkLoaiSp>();
                for (int i = 0; i < table1.Rows.Count; i++)
                {
                    TkLoaiSp kq = new TkLoaiSp();

                    kq.ID = table1.Rows[i]["PK_iCategoryID"].ToString();
                    kq.Name = table1.Rows[i]["sCategoryName"].ToString();                 
                    kq.Quantity = table1.Rows[i]["soluong"].ToString();
                    ds.Add(kq);
                }
                ViewBag.MyList = ds;

                return View();

            }
            else
            {
                return RedirectToAction("KhongDuThamQuyen", "PhanQuyen");
            }
        }    
    }
}