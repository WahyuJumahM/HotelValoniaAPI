using HotelValoniaAPI.Context;
using HotelValoniaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace HotelValoniaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KamarController : ControllerBase
    {
        private readonly KamarContext _context;

        public KamarController(IConfiguration configuration)
        {
            string connString = configuration.GetConnectionString("DefaultConnection");
            _context = new KamarContext(connString);
        }

        // GET: api/Kamar
        // Bisa diakses semua (bisa tanpa login, atau kalau mau hanya user yg login, tambahkan [Authorize])
        [Authorize]
        [HttpGet]
        public ActionResult<List<Kamar>> GetAll()
        {
            var list = _context.GetAll();
            return Ok(list);
        }

        // POST: api/Kamar
        // Hanya untuk Admin
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Insert([FromBody] Kamar kamar)
        {
            if (kamar == null || string.IsNullOrEmpty(kamar.Nama_Kamar) || kamar.Lantai <= 0 || kamar.Stok < 0 || kamar.Id_Jenis_Kamar <= 0)
            {
                return BadRequest("Data kamar tidak valid atau kurang lengkap.");
            }

            bool success = _context.Insert(kamar);
            if (success)
                return Ok(new { message = "Data kamar berhasil ditambahkan." });
            else
                return StatusCode(500, "Gagal menambahkan data kamar.");
        }

        // PUT: api/Kamar/{id}
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] Kamar kamar)
        {
            if (kamar == null)
            {
                return BadRequest("Data kamar tidak valid.");
            }

            if (id <= 0)
            {
                return BadRequest("Id kamar tidak valid.");
            }

            kamar.Id_Kamar = id;

            bool success = _context.Update(kamar);
            if (success)
                return Ok(new { message = "Data kamar berhasil diperbarui." });
            else
                return StatusCode(500, "Gagal memperbarui data kamar.");
        }


        // DELETE: api/Kamar/{id}
        // Hanya untuk Admin
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            bool success = _context.Delete(id);
            if (success)
                return Ok(new { message = $"Data kamar dengan id {id} berhasil dihapus." });
            else
                return NotFound($"Data kamar dengan id {id} tidak ditemukan.");
        }
    }

}
