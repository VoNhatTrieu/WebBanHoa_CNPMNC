using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace APIwebhoa.Models
{
    [Table("tblCheckoutDetail")]   
    
    public class ChiTietDH
    {
        [Key]
        [Column("PK_iCheckoutDetailID")]
        public int Id { get; set; }
        [Column("FK_iOrderID")]
        public int? OrderId { get; set; }
        [Column("FK_iProductID")]
        public int? ProductId { get; set; }
        [Column("iQuantity")]
        public int? Quantity { get; set; }
        [Column("fPrice")]
        public double? Price { get; set; }
        [Column("sStatus")]
        public string? Status { get; set; }
    }
}
