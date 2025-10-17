using Microsoft.AspNetCore.Mvc;
using APIwebhoa.Data;
using APIwebhoa.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace APIwebhoa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MauSPController : ControllerBase
    {
        private readonly DBContext _context;

        public MauSPController(DBContext context)
        {
            _context = context;
        }

        // GET: api/MauSP
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetAllMauSP()
        {
            try
            {
                var mauSPs = await _context.MauSPs
                    .Include(m => m.Product)
                    .Select(m => new
                    {
                        mauSPID = m.PK_iProductColorID,
                        productID = m.FK_iProductID,
                        tenMau = m.sProductColor,
                        tenSanPham = m.Product != null ? m.Product.Name : null
                    })
                    .ToListAsync();

                return Ok(mauSPs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy danh sách màu sản phẩm", error = ex.Message });
            }
        }

        // GET: api/MauSP/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetMauSPById(int id)
        {
            try
            {
                var mauSP = await _context.MauSPs
                    .Include(m => m.Product)
                    .Where(m => m.PK_iProductColorID == id)
                    .Select(m => new
                    {
                        mauSPID = m.PK_iProductColorID,
                        productID = m.FK_iProductID,
                        tenMau = m.sProductColor,
                        tenSanPham = m.Product != null ? m.Product.Name : null
                    })
                    .FirstOrDefaultAsync();

                if (mauSP == null)
                {
                    return NotFound(new { message = $"Không tìm thấy màu sản phẩm với ID {id}" });
                }

                return Ok(mauSP);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy thông tin màu sản phẩm", error = ex.Message });
            }
        }

        // GET: api/MauSP/product/5
        [HttpGet("product/{productId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetMauSPByProductId(int productId)
        {
            try
            {
                var mauSPs = await _context.MauSPs
                    .Where(m => m.FK_iProductID == productId)
                    .Select(m => new
                    {
                        mauSPID = m.PK_iProductColorID,
                        productID = m.FK_iProductID,
                        tenMau = m.sProductColor
                    })
                    .ToListAsync();

                return Ok(mauSPs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi lấy màu theo sản phẩm", error = ex.Message });
            }
        }

        // POST: api/MauSP
        [HttpPost]
        public async Task<ActionResult<object>> CreateMauSP([FromBody] MauSPCreateDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Kiểm tra sản phẩm có tồn tại không
                var productExists = await _context.Products.AnyAsync(p => p.Id == dto.ProductID);
                if (!productExists)
                {
                    return BadRequest(new { message = "Sản phẩm không tồn tại" });
                }

                var mauSP = new MauSP
                {
                    FK_iProductID = dto.ProductID,
                    sProductColor = dto.TenMau
                };

                _context.MauSPs.Add(mauSP);
                await _context.SaveChangesAsync();

                var result = new
                {
                    mauSPID = mauSP.PK_iProductColorID,
                    productID = mauSP.FK_iProductID,
                    tenMau = mauSP.sProductColor
                };

                return CreatedAtAction(nameof(GetMauSPById), new { id = mauSP.PK_iProductColorID }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi tạo màu sản phẩm", error = ex.Message });
            }
        }

        // PUT: api/MauSP/5
        [HttpPut("{id}")]
        public async Task<ActionResult<object>> UpdateMauSP(int id, [FromBody] MauSPUpdateDTO dto)
        {
            try
            {
                if (id != dto.MauSPID)
                {
                    return BadRequest(new { message = "ID không khớp" });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var mauSP = await _context.MauSPs.FindAsync(id);
                if (mauSP == null)
                {
                    return NotFound(new { message = $"Không tìm thấy màu sản phẩm với ID {id}" });
                }

                // Kiểm tra sản phẩm có tồn tại không
                var productExists = await _context.Products.AnyAsync(p => p.Id == dto.ProductID);
                if (!productExists)
                {
                    return BadRequest(new { message = "Sản phẩm không tồn tại" });
                }

                mauSP.FK_iProductID = dto.ProductID;
                mauSP.sProductColor = dto.TenMau;

                await _context.SaveChangesAsync();

                var result = new
                {
                    mauSPID = mauSP.PK_iProductColorID,
                    productID = mauSP.FK_iProductID,
                    tenMau = mauSP.sProductColor
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi cập nhật màu sản phẩm", error = ex.Message });
            }
        }

        // DELETE: api/MauSP/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMauSP(int id)
        {
            try
            {
                var mauSP = await _context.MauSPs.FindAsync(id);
                if (mauSP == null)
                {
                    return NotFound(new { message = $"Không tìm thấy màu sản phẩm với ID {id}" });
                }

                _context.MauSPs.Remove(mauSP);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Xóa màu sản phẩm thành công" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi xóa màu sản phẩm", error = ex.Message });
            }
        }
    }

    // DTO Classes
    public class MauSPCreateDTO
    {
        [Required(ErrorMessage = "ProductID là bắt buộc")]
        public int ProductID { get; set; }

        [Required(ErrorMessage = "Tên màu là bắt buộc")]
        [MaxLength(40, ErrorMessage = "Tên màu không được quá 40 ký tự")]
        public string TenMau { get; set; } = string.Empty;
    }

    public class MauSPUpdateDTO
    {
        [Required]
        public int MauSPID { get; set; }

        [Required(ErrorMessage = "ProductID là bắt buộc")]
        public int ProductID { get; set; }

        [Required(ErrorMessage = "Tên màu là bắt buộc")]
        [MaxLength(40, ErrorMessage = "Tên màu không được quá 40 ký tự")]
        public string TenMau { get; set; } = string.Empty;
    }
}
