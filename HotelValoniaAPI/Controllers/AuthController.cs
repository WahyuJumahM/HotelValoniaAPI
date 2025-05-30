using HotelValoniaAPI.Context;
using HotelValoniaAPI.Helpers;
using HotelValoniaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace HotelValoniaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly LoginContext _loginContext;

        public AuthController(IConfiguration config)
        {
            _config = config;
            string conn = _config.GetConnectionString("DefaultConnection");
            _loginContext = new LoginContext(conn);
        }

        [HttpPost("login-user")]
        public ActionResult LoginUser([FromBody] LoginModel login)
        {
            if (login == null || string.IsNullOrWhiteSpace(login.Email) || string.IsNullOrWhiteSpace(login.Password))
                return BadRequest("Email dan password harus diisi.");

            var user = _loginContext.UserLogin(login.Email, login.Password);
            if (user == null) return Unauthorized("Login user gagal.");

            var token = JwtHelper.GenerateToken(user.Email, "User", user.Id_User, user.Nama_Lengkap, _config);
            return Ok(new
            {
                token,
                role = "User",
                userId = user.Id_User,
                name = user.Nama_Lengkap
            });
        }

        [HttpPost("login-admin")]
        public ActionResult LoginAdmin([FromBody] LoginModel login)
        {
            if (login == null || string.IsNullOrWhiteSpace(login.Email) || string.IsNullOrWhiteSpace(login.Password))
                return BadRequest("Email dan password harus diisi.");

            var admin = _loginContext.AdminLogin(login.Email, login.Password);
            if (admin == null) return Unauthorized("Login admin gagal.");

            var token = JwtHelper.GenerateToken(admin.Email, "Admin", admin.Id_Admin, admin.Nama, _config);
            return Ok(new
            {
                token,
                role = "Admin",
                adminId = admin.Id_Admin,
                name = admin.Nama
            });
        }

    }
}

