using APIwebhoa.Data;
using APIwebhoa.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIwebhoa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KhuyenMaiController : ControllerBase
    {
    
        private readonly DBContext _context;
        public KhuyenMaiController(DBContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var list = _context.KhuyenMais.ToList();
            return Ok(list);
        }
        [HttpGet("{id}")]
        public IActionResult GetByid(int id)
        {
            var khuyenmai=_context.KhuyenMais.Find(id);
            if (khuyenmai == null) return NotFound();
            return Ok(khuyenmai);
        }
        [HttpPost]
        public IActionResult cretae(KhuyenMai khuyenMai)
        {
            _context.KhuyenMais.Add(khuyenMai);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetByid), new { id = khuyenMai.Id }, khuyenMai);
        }
        [HttpPut("{id}")]
        public IActionResult Update(int id, KhuyenMai km)
        {
            if(id!=km.Id) return BadRequest();
            _context.KhuyenMais.Update(km);
            _context.SaveChanges();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult delete(int id)
        {
            var km=_context.KhuyenMais.Find(id);
            if (km == null) return NotFound();
            _context.KhuyenMais.Remove(km);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
