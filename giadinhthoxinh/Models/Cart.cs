using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace giadinhthoxinh.Models
{
    public class Cart
    {
  
        
        public List<ProductInCart> lstproduct { get; set; }
        public Cart()
        {
            lstproduct = new List<ProductInCart>();
        }
        public Cart(List<ProductInCart> lstproduct)
        {
            this.lstproduct = lstproduct;
        }
    }
}