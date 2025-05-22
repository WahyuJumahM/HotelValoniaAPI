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
    [Authorize]  // Saat ini authorize tanpa role khusus, semua user bisa akses
    public class UserRoleController : ControllerBase
    {
        private readonly UserRoleContext _context;

        public UserRoleController(IConfiguration configuration)
        {
            string connString = configuration.GetConnectionString("DefaultConnection");
            _context = new UserRoleContext(connString);
        }

        // GET: api/UserRole
        [HttpGet]
        public ActionResult<List<UserRole>> GetAllUsers()
        {
            var users = _context.GetAllUsers();
            return Ok(users);
        }

        // GET: api/UserRole/5
        [HttpGet("{id}")]
        public ActionResult<UserRole> GetUserById(int id)
        {
            var user = _context.GetUserById(id);
            if (user == null)
                return NotFound($"User dengan ID {id} tidak ditemukan.");
            return Ok(user);
        }

        // POST: api/UserRole
        [HttpPost]
        public ActionResult CreateUser([FromBody] UserRole newUser)
        {
            bool success = _context.InsertUser(newUser);
            if (success)
                return CreatedAtAction(nameof(GetUserById), new { id = newUser.Id_User }, newUser);
            return BadRequest("Gagal menambah User baru.");
        }

        // PUT: api/UserRole/5
        [HttpPut("{id}")]
        public ActionResult UpdateUser(int id, [FromBody] UserRole updatedUser)
        {
            if (id != updatedUser.Id_User)
                return BadRequest("ID User tidak cocok.");

            var existUser = _context.GetUserById(id);
            if (existUser == null)
                return NotFound($"User dengan ID {id} tidak ditemukan.");

            bool success = _context.UpdateUser(updatedUser);
            if (success)
                return NoContent();
            return BadRequest("Gagal mengupdate User.");
        }

        // DELETE: api/UserRole/5
        [HttpDelete("{id}")]
        public ActionResult DeleteUser(int id)
        {
            var existUser = _context.GetUserById(id);
            if (existUser == null)
                return NotFound($"User dengan ID {id} tidak ditemukan.");

            bool success = _context.DeleteUser(id);
            if (success)
                return NoContent();
            return BadRequest("Gagal menghapus User.");
        }
    }
}
