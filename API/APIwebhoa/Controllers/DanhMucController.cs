using APIwebhoa.Data;
using APIwebhoa.Models;
using Microsoft.AspNetCore.Mvc;

namespace APIwebhoa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DanhMucController : ControllerBase
    {
        private readonly DBContext _context;

        public DanhMucController(DBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_context.DanhMucs.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var category = _context.DanhMucs.Find(id);
            if (category == null) return NotFound();
            return Ok(category);
        }

        [HttpPost]
        public IActionResult Create(DanhMuc category)
        {
            _context.DanhMucs.Add(category);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, DanhMuc category)
        {
            if (id != category.Id) return BadRequest();
            _context.DanhMucs.Update(category);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var category = _context.DanhMucs.Find(id);
            if (category == null) return NotFound();
            _context.DanhMucs.Remove(category);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
