using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace APIwebhoa.Models
{
    [Table("tblPromote")]

    public class KhuyenMai
    {
        [Key]
        [Column("PK_iPromoteID")]
        public int Id { get; set; }
        [Column("sPromoteName")]
        public string Name { get; set; }
        [Column("sPromoteRate")]
        public double? Rate { get; set; }

        [Column("dtStartDay")]
        
        public DateTime? StartDate { get; set; }
        [Column("dtEndDay")]
        public DateTime? EndDate { get; set; }
       
    }
}
