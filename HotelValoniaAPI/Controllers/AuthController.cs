using HotelValoniaAPI.Context;
using HotelValoniaAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HotelValoniaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly LoginContext _loginContext;
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
            string connString = _configuration.GetConnectionString("DefaultConnection");
            _loginContext = new LoginContext(connString);
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginModel login)
        {
            if (login == null || string.IsNullOrEmpty(login.Email) || string.IsNullOrEmpty(login.Password))
                return BadRequest("Email dan password harus diisi.");

            // Cek UserRole login
            var user = _loginContext.UserLogin(login.Email, login.Password);
            if (user != null)
            {
                var token = GenerateJwtToken(user.Email, "User");
                return Ok(new { token, role = "User", userId = user.Id_User, name = user.Nama_Lengkap });
            }

            // Cek AdminRole login (tanpa password, karena AdminRole tidak ada password)
            var admin = _loginContext.AdminLogin(login.Email, login.Password);
            if (admin != null)
            {
                var token = GenerateJwtToken(admin.Email, "Admin");
                return Ok(new { token, role = "Admin", adminId = admin.Id_Admin, name = admin.Nama });
            }

            return Unauthorized("Email atau password salah.");
        }

        private string GenerateJwtToken(string email, string role)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role)
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    // Model login untuk input
    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
