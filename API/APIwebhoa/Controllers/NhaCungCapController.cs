using APIwebhoa.Data;
using APIwebhoa.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIwebhoa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NhaCungCapController : ControllerBase
    { 
        private readonly DBContext _context;
        public NhaCungCapController(DBContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Getall()
        {
            var list=_context.NhaCungCaps.ToList();
            return Ok(list);
        }
        [HttpGet("{id}")]
        public IActionResult GetByid(int id)
        {
            var ncc = _context.NhaCungCaps.Find(id);
            if (ncc == null) return NotFound();
            return Ok(ncc);
        }
        [HttpPost]
        public IActionResult Create(NhaCungCap sup)
        {
            _context.NhaCungCaps.Add(sup);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetByid), new { id = sup.Id }, sup);
        }
        [HttpPut("{id}")]
        public IActionResult update(int id,NhaCungCap sup)
        {
            if (id != sup.Id) return BadRequest();
            _context.NhaCungCaps.Update(sup);
            _context.SaveChanges();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var sup = _context.NhaCungCaps.Find(id);
            if (sup == null) return NotFound();
            _context.NhaCungCaps.Remove(sup);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
