using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIwebhoa.Models
{
    [Table("tblCategory")]
    public class DanhMuc
    {
        [Key]
        [Column("PK_iCategoryID")]
        public int Id { get; set; }

        [Column("sCategoryName")]
        public string Name { get; set; }

        // Navigation
        public ICollection<SanPham> Products { get; set; }
    }
}
