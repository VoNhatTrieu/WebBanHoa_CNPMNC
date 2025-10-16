using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIwebhoa.Models
{
    [Table("tblProductColor")]
    public class MauSP
    {
        [Key]
        [Column("PK_iProductColorID")]
        public int PK_iProductColorID { get; set; }

        [Column("FK_iProductID")]
        public int? FK_iProductID { get; set; }   // Cho phép NULL vì SQL cho phép NULL

        [Column("sProductColor")]
        [MaxLength(40)]
        public string? sProductColor { get; set; }

        // Khóa ngoại liên kết đến sản phẩm
        [ForeignKey("FK_iProductID")]
        public SanPham? Product { get; set; }
    }
}