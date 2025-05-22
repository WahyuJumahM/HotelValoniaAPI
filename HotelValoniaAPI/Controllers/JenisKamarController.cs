using HotelValoniaAPI.Context;
using HotelValoniaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace HotelValoniaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JenisKamarController : ControllerBase
    {
        private readonly JenisKamarContext _context;

        public JenisKamarController(IConfiguration configuration)
        {
            string connString = configuration.GetConnectionString("DefaultConnection");
            _context = new JenisKamarContext(connString);
        }

        // GET: api/JenisKamar
        [HttpGet]
        public ActionResult<List<JenisKamar>> GetAll()
        {
            var result = _context.GetAll();
            return Ok(result);
        }

        // POST: api/JenisKamar
        [HttpPost]
        public ActionResult Insert([FromBody] JenisKamar jenisKamar)
        {
            if (jenisKamar == null || string.IsNullOrEmpty(jenisKamar.Deskripsi) || jenisKamar.Harga <= 0 || jenisKamar.Kapasitas <= 0)
            {
                return BadRequest("Data jenis kamar tidak lengkap atau tidak valid.");
            }

            bool success = _context.Insert(jenisKamar);
            if (success)
                return Ok(new { message = "Jenis kamar berhasil ditambahkan." });
            else
                return StatusCode(500, "Gagal menambahkan jenis kamar.");
        }

        // PUT: api/JenisKamar
        [HttpPut]
        public ActionResult Update([FromBody] JenisKamar jenisKamar)
        {
            if (jenisKamar == null || jenisKamar.Id_Jenis_Kamar <= 0)
            {
                return BadRequest("Id jenis kamar tidak valid.");
            }

            bool success = _context.Update(jenisKamar);
            if (success)
                return Ok(new { message = "Jenis kamar berhasil diperbarui." });
            else
                return StatusCode(500, "Gagal memperbarui jenis kamar.");
        }

        // DELETE: api/JenisKamar/{id}
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            bool success = _context.Delete(id);
            if (success)
                return Ok(new { message = $"Jenis kamar dengan id {id} berhasil dihapus." });
            else
                return NotFound($"Jenis kamar dengan id {id} tidak ditemukan.");
        }
    }
}
