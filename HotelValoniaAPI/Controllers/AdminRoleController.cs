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
    [Authorize] // Semua role bisa akses sementara
    public class AdminRoleController : ControllerBase
    {
        private readonly AdminRoleContext _context;

        public AdminRoleController(IConfiguration configuration)
        {
            string connString = configuration.GetConnectionString("DefaultConnection");
            _context = new AdminRoleContext(connString);
        }

        // GET: api/AdminRole
        [HttpGet]
        public ActionResult<List<AdminRole>> GetAllAdmins()
        {
            var admins = _context.GetAllAdmins();
            return Ok(admins);
        }

        // GET: api/AdminRole/5
        [HttpGet("{id}")]
        public ActionResult<AdminRole> GetAdminById(int id)
        {
            var admin = _context.GetAdminById(id);
            if (admin == null)
                return NotFound($"Admin dengan ID {id} tidak ditemukan.");
            return Ok(admin);
        }

        // POST: api/AdminRole
        [HttpPost]
        public ActionResult CreateAdmin([FromBody] AdminRole newAdmin)
        {
            bool success = _context.InsertAdmin(newAdmin);
            if (success)
                return CreatedAtAction(nameof(GetAdminById), new { id = newAdmin.Id_Admin }, newAdmin);
            return BadRequest("Gagal menambah Admin baru.");
        }

        // PUT: api/AdminRole/5
        [HttpPut("{id}")]
        public ActionResult UpdateAdmin(int id, [FromBody] AdminRole updatedAdmin)
        {
            if (id != updatedAdmin.Id_Admin)
                return BadRequest("ID Admin tidak cocok.");

            var existAdmin = _context.GetAdminById(id);
            if (existAdmin == null)
                return NotFound($"Admin dengan ID {id} tidak ditemukan.");

            bool success = _context.UpdateAdmin(updatedAdmin);
            if (success)
                return NoContent();
            return BadRequest("Gagal mengupdate Admin.");
        }

        // DELETE: api/AdminRole/5
        [HttpDelete("{id}")]
        public ActionResult DeleteAdmin(int id)
        {
            var existAdmin = _context.GetAdminById(id);
            if (existAdmin == null)
                return NotFound($"Admin dengan ID {id} tidak ditemukan.");

            bool success = _context.DeleteAdmin(id);
            if (success)
                return NoContent();
            return BadRequest("Gagal menghapus Admin.");
        }
    }
}
