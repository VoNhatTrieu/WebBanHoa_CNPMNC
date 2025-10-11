using APIwebhoa.Data;
using APIwebhoa.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIwebhoa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NguyenLieuController : ControllerBase
    {
        private readonly DBContext _context;
        public NguyenLieuController(DBContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var list = _context.NguyenLieus.ToList();
            return Ok(list);
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var nguyenlieu=_context.NguyenLieus.Find(id);
            if (nguyenlieu == null)
            {
                return NotFound();
            }
            return Ok(nguyenlieu);
        }
        [HttpPost]
        public IActionResult Create(NguyenLieu nguyenLieu)
        {
            _context.NguyenLieus.Add(nguyenLieu);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = nguyenLieu.Id }, nguyenLieu);
        }
        [HttpPut("{id}")]
        public IActionResult Update(int id, NguyenLieu nguyenLieu)
        {
            if(id!=nguyenLieu.Id) return BadRequest();
            _context.NguyenLieus.Update(nguyenLieu);
            _context.SaveChanges();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult Deletee(int id)
        {
            var nl=_context.NguyenLieus.Find(id);
            if(nl==null) return NotFound();
            _context.NguyenLieus.Remove(nl);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
