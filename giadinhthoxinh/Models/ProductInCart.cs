using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace giadinhthoxinh.Models
{
    public class ProductInCart
    {
        /*  giadinhthoxinhEntities db = new giadinhthoxinhEntities();
    public int iMasp { get; set; }
    public string sTensp { get; set; }
    public string sAnhBia { get; set; }
    public double dDonGia { get; set; }
    public int iSoLuong { get; set; }

    public string unit { get; set; }
    public double ThanhTien
    {
        get { return iSoLuong * dDonGia ; }
    }
    //Hàm tạo cho giỏ hàng
    public Cart(int Masp,int soluong)
    {
        iMasp = Masp;
        tblProduct sp = db.tblProducts.Single(n => n.PK_iProductID == iMasp);
        sTensp = sp.sProductName;
        sAnhBia = sp.sImage;
        dDonGia = double.Parse(sp.fPrice.ToString());
        iSoLuong = soluong;
        unit = sp.sUnit;
    }*/
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string LinkImage { get; set; }
        public double Price { get; set; }
        public string unit { get; set; }
        public int Quatity { get; set; }
        public double ThanhTien
        {
            get { return Quatity * Price; }
        }
        public ProductInCart(int productid, int quatity)
        {
            giadinhthoxinhEntities1 db = new giadinhthoxinhEntities1();
            tblProduct sp = db.tblProducts.Single(n => n.PK_iProductID == productid);
            ProductID = sp.PK_iProductID;
            ProductName = sp.sProductName;
            LinkImage = sp.sImage;
            Price = double.Parse(sp.fPrice.ToString());
            Quatity = quatity;
            unit = sp.sUnit;

        }
        public ProductInCart() { }
    }
    




}