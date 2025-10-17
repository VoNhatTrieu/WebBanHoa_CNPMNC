using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIwebhoa.Models
{
    [Table("tblMaterSize")]
    public class MaterSize
    {
        [Key]
        [Column("PK_iMaterSizeID")]
        public int Id { get; set; }

        [Column("FK_iMaterialID")]
        public int MaterialId { get; set; }

        [Column("sMaterSize")]
        public string MaterSizeName { get; set; }
    }
}
