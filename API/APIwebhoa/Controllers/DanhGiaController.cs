using APIwebhoa.Data;
using APIwebhoa.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIwebhoa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DanhGiaController : ControllerBase
    {
        private readonly DBContext _context;

        public DanhGiaController(DBContext context)
        {
            _context = context;
        }

        // ✅ GET: api/DanhGia
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DanhGia>>> GetDanhGias()
        {
            var reviews = await _context.DanhGias
                .Include(r => r.Product)
                .Include(r => r.TaiKhoan)
                .ToListAsync();

            return Ok(reviews);
        }

        // ✅ GET: api/DanhGia/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<DanhGia>> GetDanhGia(int id)
        {
            var review = await _context.DanhGias
                .Include(r => r.Product)
                .Include(r => r.TaiKhoan)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (review == null)
                return NotFound(new { message = "Không tìm thấy đánh giá" });

            return Ok(review);
        }

        // ✅ POST: api/DanhGia
        [HttpPost]
        public async Task<ActionResult<DanhGia>> CreateDanhGia(DanhGia review)
        {
            review.ReviewDate = DateTime.Now;

            _context.DanhGias.Add(review);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDanhGia), new { id = review.Id }, review);
        }

        // ✅ PUT: api/DanhGia/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDanhGia(int id, DanhGia review)
        {
            if (id != review.Id)
                return BadRequest(new { message = "ID không khớp" });

            _context.Entry(review).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Cập nhật đánh giá thành công" });
        }

        // ✅ DELETE: api/DanhGia/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDanhGia(int id)
        {
            var review = await _context.DanhGias.FindAsync(id);
            if (review == null)
                return NotFound(new { message = "Không tìm thấy đánh giá" });

            _context.DanhGias.Remove(review);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Xóa đánh giá thành công" });
        }
    }
}
