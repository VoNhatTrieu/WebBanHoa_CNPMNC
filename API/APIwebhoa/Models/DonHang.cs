using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace APIwebhoa.Models
{
    [Table("tblOrder")]

    public class DonHang
    {
        [Key]
        [Column("PK_iOrderID")]
        public int Id { get; set; }
        [Column("FK_iAccountID")]
        public int? AccountId { get; set; }
        [Column("sCustomerName")]
        public string ? CustomerName { get; set; }
        [Column("sCustomerPhone")]
        public string? CustomerPhone { get; set; }
        [Column("sDeliveryAddress")]
        public string? DeliveryAddress { get; set; }
        [Column("dInvoidDate")]
        public DateTime? InvoidDate { get; set; }
        [Column("sBiller")]
        public string? Biller { get; set; }
        [Column("iDeliveryMethod")]
        public int? DeliveryMethod { get; set; }
        [Column("fSurcharge")]
        public double? Surcharge { get; set; }
        [Column("iPaid")]
        public int? Paid { get; set; }
        [Column("sState")]
        public string? State { get; set; }
        [Column("iTotal")]
        public int? Total { get; set; }
    }
}
