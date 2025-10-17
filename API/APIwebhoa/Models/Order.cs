using System;

namespace APIwebhoa.Models
{
    public class Order
    {
        public int PK_iOrderID { get; set; }
        public int FK_iAccountID { get; set; }
        public string sCustomerName { get; set; }
        public string sCustomerPhone { get; set; }
        public string sDeliveryAddress { get; set; }
        public DateTime dInvoidDate { get; set; }
        public string sBiller { get; set; }
        public int? iDeliveryMethod { get; set; }
        public double? fSurcharge { get; set; }
        public int? iPaid { get; set; }
        public string sState { get; set; }
        public int? iTotal { get; set; }
    }
}
