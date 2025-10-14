using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIwebhoa.Models
{
    [Table("tblReview")]
    public class DanhGia
    {
        [Key]
        [Column("PK_iReviewID")]
        public int Id { get; set; }

        [Column("FK_iProductID")]
        public int ProductId { get; set; }

        [Column("FK_iAccountID")]
        public int UserId { get; set; }

        [Column("iRating")]
        public int Rating { get; set; }

        [Column("sComment")]
        public string? Comment { get; set; }

        [Column("dReviewDate")]
        public DateTime? ReviewDate { get; set; }

        // Navigation
        [ForeignKey("ProductId")]
        public SanPham? Product { get; set; }

        [ForeignKey("UserId")]
        public TaiKhoan? TaiKhoan { get; set; }
    }
}
