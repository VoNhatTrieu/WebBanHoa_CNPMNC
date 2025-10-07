using System.Diagnostics.Contracts;
using APIwebhoa.Data;
using APIwebhoa.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIwebhoa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaiKhoanController : ControllerBase
    {
        private readonly DBContext _context;
        public TaiKhoanController(DBContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var list = _context.TaiKhoans.Include(tk => tk.PhanQuyen).ToList();
            return Ok(list);
        }
      
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var tk = _context.TaiKhoans
                .Include(tk => tk.PhanQuyen)
                .FirstOrDefault(tk => tk.Id == id);
            if (tk == null) return NotFound();
            return Ok(tk);
        }
        [HttpPost]
        public IActionResult Create(TaiKhoan tk)
        {
            _context.TaiKhoans.Add(tk);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = tk.Id }, tk);
        }
        [HttpPut("{id}")]
        public IActionResult Update(int id, TaiKhoan tk)
        {
            if (id != tk.Id) return BadRequest();
            _context.TaiKhoans.Update(tk);
            _context.SaveChanges();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var tk = _context.TaiKhoans.Find(id);
            if (tk == null) return NotFound();
            _context.TaiKhoans.Remove(tk);
            _context.SaveChanges();
            return NoContent();
        }
    } 
}
