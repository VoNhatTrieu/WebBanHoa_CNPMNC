using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIwebhoa.Models
{
    [Table("tblSupplier")]
    public class NhaCungCap
    {
        [Key]
        [Column("PK_iSupplierID")]
        public int Id { get; set; }
        [Column("sSupplierName")]
        public string Name { get; set; }
        [Column("sPhone")]
        public string? Phone { get; set; }
        [Column("sAddress")]
        public string?Address { get; set; }

    }
}
