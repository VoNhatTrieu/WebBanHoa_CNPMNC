using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace APIwebhoa.Models
{
    [Table("tblMaterial")]
    public class NguyenLieu
    {
        [Key]
        [Column("PK_iMaterialID")]
        public int Id { get; set; }
        [Column("sMaterialName")]
        public string Name { get; set; }
        [Column("sDescribe")]
        public string? Describe { get; set; }
        [Column("iQuatity")]
        public int? Quantity { get; set; }
        [Column("sUnit")]
        public string? Unit { get; set; }
    }
}
