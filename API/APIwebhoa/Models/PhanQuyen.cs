using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIwebhoa.Models
{
    [Table("tblPermission")]
    public class PhanQuyen
    {
        [Key]
        [Column("PK_iPermissionID")]
        public int Id { get; set; }
        [Column("sPermissionName")]
        public string PermissionName { get; set; }
        public List<TaiKhoan>? TaiKhoans { get; set; }

    }
}
