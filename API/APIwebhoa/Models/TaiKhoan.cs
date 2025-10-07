using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIwebhoa.Models
{
    [Table("tblUser")]
    public class TaiKhoan
    {
        [Key]
        [Column("PK_iAccountID")]
        public int Id { get; set; }

        [Column("sUserName")]
        public string UserName { get; set; }

        [Column("sPass")]
        public string Password { get; set; }

        [Column("sEmail")]
        public string? Email { get; set; }

        [Column("sPhone")]
        public string? Phone { get; set; }

        [Column("sAddress")]
        public string? Address { get; set; }

        [Column("iState")]
        public int? State { get; set; }

        [Column("FK_iPermissionID")]
        public int? PermissionId { get; set; }

        [ForeignKey("PermissionId")]
        public PhanQuyen? PhanQuyen { get; set; }
    }
}
