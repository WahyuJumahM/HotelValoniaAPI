using HotelValoniaAPI.Context;
using HotelValoniaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace HotelValoniaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly StatusContext _context;

        public StatusController(IConfiguration configuration)
        {
            string connString = configuration.GetConnectionString("DefaultConnection");
            _context = new StatusContext(connString);
        }

        // GET: api/Status
        [HttpGet]
        public ActionResult<List<Status>> GetAll()
        {
            var list = _context.GetAll();
            return Ok(list);
        }

        // POST: api/Status
        [HttpPost]
        public ActionResult Insert([FromBody] Status status)
        {
            if (status == null || string.IsNullOrEmpty(status.Nama_Status))
                return BadRequest("Nama status harus diisi.");

            bool success = _context.Insert(status);
            if (success)
                return Ok(new { message = "Status berhasil ditambahkan." });
            else
                return StatusCode(500, "Gagal menambahkan status.");
        }

        // PUT: api/Status
        [HttpPut]
        public ActionResult Update([FromBody] Status status)
        {
            if (status == null || status.Id_Status <= 0 || string.IsNullOrEmpty(status.Nama_Status))
                return BadRequest("Data status tidak valid.");

            bool success = _context.Update(status);
            if (success)
                return Ok(new { message = "Status berhasil diperbarui." });
            else
                return StatusCode(500, "Gagal memperbarui status.");
        }

        // DELETE: api/Status/{id}
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            bool success = _context.Delete(id);
            if (success)
                return Ok(new { message = $"Status dengan id {id} berhasil dihapus." });
            else
                return NotFound($"Status dengan id {id} tidak ditemukan.");
        }
    }
}
