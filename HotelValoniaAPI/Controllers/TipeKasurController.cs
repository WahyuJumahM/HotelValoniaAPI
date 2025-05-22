using HotelValoniaAPI.Context;
using HotelValoniaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace HotelValoniaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipeKasurController : ControllerBase
    {
        private readonly TipeKasurContext _context;

        public TipeKasurController(IConfiguration configuration)
        {
            string connString = configuration.GetConnectionString("DefaultConnection");
            _context = new TipeKasurContext(connString);
        }

        // GET: api/TipeKasur
        [HttpGet]
        public ActionResult<List<TipeKasur>> GetAll()
        {
            var list = _context.GetAll();
            return Ok(list);
        }

        // POST: api/TipeKasur
        [HttpPost]
        public ActionResult Insert([FromBody] TipeKasur tipe)
        {
            if (tipe == null || string.IsNullOrEmpty(tipe.Tipe_Kasur))
                return BadRequest("Tipe kasur harus diisi.");

            bool success = _context.Insert(tipe);
            if (success)
                return Ok(new { message = "Tipe kasur berhasil ditambahkan." });
            else
                return StatusCode(500, "Gagal menambahkan tipe kasur.");
        }

        // PUT: api/TipeKasur
        [HttpPut]
        public ActionResult Update([FromBody] TipeKasur tipe)
        {
            if (tipe == null || tipe.Id_Tipe_Kasur <= 0 || string.IsNullOrEmpty(tipe.Tipe_Kasur))
                return BadRequest("Data tipe kasur tidak valid.");

            bool success = _context.Update(tipe);
            if (success)
                return Ok(new { message = "Tipe kasur berhasil diperbarui." });
            else
                return StatusCode(500, "Gagal memperbarui tipe kasur.");
        }

        // DELETE: api/TipeKasur/{id}
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            bool success = _context.Delete(id);
            if (success)
                return Ok(new { message = $"Tipe kasur dengan id {id} berhasil dihapus." });
            else
                return NotFound($"Tipe kasur dengan id {id} tidak ditemukan.");
        }
    }
}
