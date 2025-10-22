using APIwebhoa.Data;
using APIwebhoa.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIwebhoa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChiTietDHController : ControllerBase
    {
        private readonly DBContext _context;
        public ChiTietDHController(DBContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var list = _context.ChiTietDonHangs.ToList();
            return Ok(list);
        }
        [HttpGet("{id}")]
        public IActionResult getById(int id)
        {
            var ctdh=_context.ChiTietDonHangs.Find(id);
            if (ctdh == null) return NotFound();
            return Ok(ctdh);
        }
        [HttpPost]
        public IActionResult Create(ChiTietDH dh)
        {
           _context.ChiTietDonHangs.Add(dh);
            _context.SaveChanges();
           return CreatedAtAction(nameof(getById), new { id = dh.Id }, dh);
        }
        [HttpPut("{id}")]
        public IActionResult Update(int id, ChiTietDH dh)
        {
            if (id != dh.Id) return BadRequest();
            _context.ChiTietDonHangs.Update(dh);
            _context.SaveChanges();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var ctdh = _context.ChiTietDonHangs.Find(id);
            if (ctdh == null) return NotFound();
            _context.ChiTietDonHangs.Remove(ctdh);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
