using HotelValoniaAPI.Context;
using HotelValoniaAPI.Models;
using Microsoft.AspNetCore.Mvc;
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
        [HttpGet]
        public ActionResult<List<Booking>> GetAll()
        {
            var bookings = _bookingContext.GetAll();
            return Ok(bookings);
        }

        // POST: api/Booking
        [HttpPost]
        public ActionResult Insert([FromBody] Booking booking)
        {
            if (booking == null || booking.Cek_In == default || booking.Cek_Out == default
                || booking.Id_User <= 0 || booking.Id_Admin <= 0 || booking.Id_Kamar <= 0 || booking.Id_Status <= 0)
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
