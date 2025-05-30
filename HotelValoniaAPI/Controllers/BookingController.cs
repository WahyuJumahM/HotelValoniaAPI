using HotelValoniaAPI.Context;
using HotelValoniaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Security.Claims;

using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace HotelValoniaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly BookingContext _bookingContext;

        public BookingController(IConfiguration configuration)
        {
            string connString = configuration.GetConnectionString("DefaultConnection");
            _bookingContext = new BookingContext(connString);
        }

        // GET: api/Booking
        [Authorize]
        [HttpGet]
        public ActionResult<List<Booking>> GetAll()
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            // Ambil nilai dari "userId" yang kamu set di JWT (bukan dari ClaimTypes.NameIdentifier)
            var userIdClaim = User.FindFirst("userId")?.Value;
            int.TryParse(userIdClaim, out int userId);

            List<Booking> bookings;

            if (role == "Admin")
            {
                bookings = _bookingContext.GetAll(); // Tampilkan semua
            }
            else if (role == "User")
            {
                bookings = _bookingContext.GetByUserId(userId); // Hanya untuk user terkait
            }
            else
            {
                return Forbid("Role tidak diizinkan.");
            }

            return Ok(bookings);
        }


        // POST: api/Booking
        [Authorize(Roles = "User")]
        [HttpPost]
        public ActionResult Insert([FromBody] Booking booking)
        {
            // Ambil klaim userId dari token JWT
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized("User tidak valid.");
            }

            // Isi otomatis Id_User dan Id_Admin
            booking.Id_User = userId;
            booking.Id_Admin = 1;
            booking.Id_Status = 1;

            // Validasi field lainnya
            if (booking.Cek_In == default || booking.Cek_Out == default || booking.Id_Kamar <= 0 || booking.Id_Status <= 0)
            {
                return BadRequest("Data booking tidak lengkap atau invalid.");
            }

            bool success = _bookingContext.Insert(booking);
            if (success)
                return Ok(new { message = "Booking berhasil disimpan." });
            else
                return StatusCode(500, "Gagal menyimpan booking.");
        }


        // DELETE: api/Booking/{id}
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            bool success = _bookingContext.Delete(id);
            if (success)
                return Ok(new { message = $"Booking dengan id {id} berhasil dihapus." });
            else
                return NotFound($"Booking dengan id {id} tidak ditemukan.");
        }
    }
}
