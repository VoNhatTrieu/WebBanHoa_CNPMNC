using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIwebhoa.Models
{
    [Table("tblProduct")]
    public class SanPham
    {
        [Key]
        [Column("PK_iProductID")]
        public int Id { get; set; }

        [Column("FK_iCategoryID")]
        public int? CategoryId { get; set; }   // cho phép NULL

        [Column("FK_iPromoteID")]
        public int? PromoteId { get; set; }    // cho phép NULL

        [Column("sProductName")]
        public string Name { get; set; }

        [Column("sDescribe")]
        public string? Describe { get; set; }

        [Column("fPrice")]
        public double? Price { get; set; }     // cho phép NULL

        [Column("sColor")]
        public string? Color { get; set; }

        [Column("sSize")]
        public string? Size { get; set; }

        [Column("sImage")]
        public string? Image { get; set; }

        [Column("sUnit")]
        public string? Unit { get; set; }

        [Column("iQuantity")]
        public int? Quantity { get; set; }     // cho phép NULL

        // Navigation property
        public Category? Category { get; set; }
    }
}
