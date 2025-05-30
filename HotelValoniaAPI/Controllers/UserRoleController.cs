using HotelValoniaAPI.Context;
using HotelValoniaAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Security.Claims;

namespace HotelValoniaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        [Authorize]
        public ActionResult<List<UserRole>> GetAllUsers()
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var userIdClaim = User.FindFirst("userId")?.Value;
            int.TryParse(userIdClaim, out int userId);

            // Ambil data sesuai role dari token, bukan dari kolom role di database
            var users = _context.GetAllUsers(role, userId);

            if (users == null || users.Count == 0)
                return NotFound("Data user tidak ditemukan.");

            return Ok(users);
        }




        // GET: api/UserRole/5
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public ActionResult<UserRole> GetUserById(int id)
        {
            var user = _context.GetUserById(id);
            if (user == null)
                return NotFound($"User dengan ID {id} tidak ditemukan.");
            return Ok(user);
        }

        // POST: api/UserRole
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult CreateUser([FromBody] UserRole newUser)
        {
            bool success = _context.InsertUser(newUser);
            if (success)
                return CreatedAtAction(nameof(GetUserById), new { id = newUser.Id_User }, newUser);
            return BadRequest("Gagal menambah User baru.");
        }

        // PUT: api/UserRole/5
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
