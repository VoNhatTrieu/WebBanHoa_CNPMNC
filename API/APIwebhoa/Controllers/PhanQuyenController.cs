using APIwebhoa.Data;
using APIwebhoa.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIwebhoa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhanQuyenController : ControllerBase
    {
        private readonly DBContext _context;
        public PhanQuyenController(DBContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var list = _context.PhanQuyens.Include(pq => pq.TaiKhoans).ToList();
            return Ok(list);
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var phanquyen = _context.PhanQuyens.Include(pq => pq.TaiKhoans).FirstOrDefault(pq => pq.Id == id);
            if (phanquyen == null) return NotFound();
            return Ok(phanquyen);
        }
        [HttpPost]
        public IActionResult Create(Models.PhanQuyen pq)
        {
            _context.PhanQuyens.Add(pq);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = pq.Id }, pq);
        }
        [HttpPut("{id}")]
        public IActionResult Update(int id, PhanQuyen phanQuyen)
        {
           if(id!=phanQuyen.Id) return BadRequest();
           _context.Entry(phanQuyen).State = EntityState.Modified;
            _context.SaveChanges();
            return NoContent();

        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var pq = _context.PhanQuyens.Find(id);
            if (pq == null) return NotFound();
            _context.PhanQuyens.Remove(pq);
            _context.SaveChanges();
            return NoContent();
        }

    }
}
