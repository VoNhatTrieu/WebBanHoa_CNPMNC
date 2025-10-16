using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIwebhoa.Data;
using APIwebhoa.Models;

namespace APIwebhoa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterSizeController : ControllerBase
    {
        private readonly DBContext _context;

        public MaterSizeController(DBContext context)
        {
            _context = context;
        }

        // GET: api/MaterSize
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MaterSize>>> GetAll()
        {
            return await _context.MaterSizes.ToListAsync();
        }

        // GET: api/MaterSize/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MaterSize>> GetById(int id)
        {
            var size = await _context.MaterSizes.FindAsync(id);
            if (size == null)
                return NotFound(new { message = $"Không tìm thấy kích thước có ID {id}" });

            return Ok(size);
        }

        // POST: api/MaterSize
        [HttpPost]
        public async Task<ActionResult<MaterSize>> Create(MaterSize model)
        {
            _context.MaterSizes.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
        }

        // PUT: api/MaterSize/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, MaterSize model)
        {
            if (id != model.Id)
                return BadRequest(new { message = "ID không khớp" });

            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(model);
        }

        // DELETE: api/MaterSize/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var size = await _context.MaterSizes.FindAsync(id);
            if (size == null)
                return NotFound(new { message = $"Không tìm thấy kích thước có ID {id}" });

            _context.MaterSizes.Remove(size);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đã xóa kích thước thành công" });
        }
    }
}
