using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HotelValoniaAPI.Helpers
{
    public static class JwtHelper
    {
        public static string GenerateToken(string email, string role, int userId, string name, IConfiguration config)
        {
            // Validasi parameter minimal
            if (string.IsNullOrEmpty(email)) throw new ArgumentNullException(nameof(email));
            if (string.IsNullOrEmpty(role)) throw new ArgumentNullException(nameof(role));
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (config == null) throw new ArgumentNullException(nameof(config));

            // Ambil key dari konfigurasi
            var keyString = config["Jwt:Key"];
            if (string.IsNullOrEmpty(keyString))
                throw new ArgumentException("JWT Key tidak ditemukan pada konfigurasi.");

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role),
                new Claim(ClaimTypes.Name, name),
                new Claim("userId", userId.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(3),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
