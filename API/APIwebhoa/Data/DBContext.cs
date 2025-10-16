using APIwebhoa.Models;
using Microsoft.EntityFrameworkCore;
namespace APIwebhoa.Data

{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
        }
        public DbSet<Models.Category> Categories { get; set; }
        public DbSet<Models.SanPham> Products { get; set; }
        public DbSet<Models.TaiKhoan> TaiKhoans { get; set; }
        public DbSet<Models.PhanQuyen> PhanQuyens { get; set; }
        public DbSet<Models.NhaCungCap> NhaCungCaps { get; set; }
        public DbSet<Models.NguyenLieu>NguyenLieus  { get; set; }
        public DbSet<Models.KhuyenMai> KhuyenMais { get; set; }
        public DbSet<DanhGia> DanhGias { get; set; }
        public DbSet<MauSP> MauSPs { get; set; }
        
        public DbSet<MaterSize> MaterSizes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SanPham>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);
        }
    }
}
