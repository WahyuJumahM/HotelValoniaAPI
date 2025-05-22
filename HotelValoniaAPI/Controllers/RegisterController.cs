using HotelValoniaAPI.Context;
using HotelValoniaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace HotelValoniaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly RegisterContext _registerContext;
        private readonly IConfiguration _configuration;

        public RegisterController(IConfiguration configuration)
        {
            _configuration = configuration;
            string connString = _configuration.GetConnectionString("DefaultConnection");
            _registerContext = new RegisterContext(connString);
        }

        [HttpPost]
        public ActionResult Register([FromBody] UserRole user)
        {
            if (user == null ||
                string.IsNullOrEmpty(user.Nama_Lengkap) ||
                string.IsNullOrEmpty(user.Email) ||
                string.IsNullOrEmpty(user.Password) ||
                user.NIK <= 0 ||
                user.No_Handphone <= 0)
            {
                return BadRequest("Data user tidak lengkap atau invalid.");
            }

            // Bisa tambahkan validasi email, cek apakah email sudah terdaftar dsb di sini

            bool success = _registerContext.RegisterUser(user);
            if (success)
            {
                return Ok(new { message = "Register berhasil" });
            }
            else
            {
                return StatusCode(500, "Gagal menyimpan data user.");
            }
        }
    }
}
