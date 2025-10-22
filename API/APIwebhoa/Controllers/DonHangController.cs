using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIwebhoa.Data;
using APIwebhoa.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APIwebhoa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonHangController : ControllerBase
    {
        private readonly DBContext _context;

        public DonHangController(DBContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var list = _context.DonHangs.ToList();
            return Ok(list);
        }
        [HttpGet("{id}")]
        public IActionResult Getyid(int id)
        {
            var dh=_context.DonHangs.Find(id);
            if (dh == null) return NotFound();
            return Ok(dh);
        }
        [HttpPost]
        public IActionResult Create(DonHang dh)
        {
            _context.DonHangs.Add(dh);
            _context.SaveChanges();
            return CreatedAtAction(nameof(Getyid), new { id = dh.Id }, dh);
        }
        [HttpPut("{id}")]
        public IActionResult update(int id,DonHang dh)
        {
            if(id != dh.Id) return BadRequest();
            _context.DonHangs.Update(dh);
            _context.SaveChanges();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var donhang=_context.DonHangs.Find(id);
            if(donhang == null) return NotFound();
            _context.DonHangs.Remove(donhang);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
