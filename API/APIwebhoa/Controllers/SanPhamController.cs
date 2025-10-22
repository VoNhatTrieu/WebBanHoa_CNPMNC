using APIwebhoa.Data;
using APIwebhoa.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIwebhoa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SanPhamController : ControllerBase
    {
        private readonly DBContext _context;

        public SanPhamController(DBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var list = _context.SanPhams.Include(p => p.Category).ToList();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var product = _context.SanPhams
                .Include(p => p.Category)
                .FirstOrDefault(p => p.Id == id);

            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpPost]
        public IActionResult Create(SanPham product)
        {
            _context.SanPhams.Add(product);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, SanPham product)
        {
            if (id != product.Id) return BadRequest();
            _context.SanPhams.Update(product);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var product = _context.SanPhams.Find(id);
            if (product == null) return NotFound();
            _context.SanPhams.Remove(product);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
