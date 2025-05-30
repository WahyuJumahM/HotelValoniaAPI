using HotelValoniaAPI.Context;
using HotelValoniaAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace HotelValoniaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
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

        // PUT: api/Status/{id}
        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] Status status)
        {
            if (status == null || string.IsNullOrEmpty(status.Nama_Status))
                return BadRequest("Nama status harus diisi.");

            if (id <= 0)
                return BadRequest("Id status tidak valid.");

            status.Id_Status = id;

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
