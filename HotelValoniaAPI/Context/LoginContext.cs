using System;
using Npgsql;
using HotelValoniaAPI.Helpers;
using HotelValoniaAPI.Models;

namespace HotelValoniaAPI.Context
{
    public class LoginContext
    {
        private readonly sqlDBHelper dbHelper;

        public LoginContext(string connectionString)
        {
            dbHelper = new sqlDBHelper(connectionString);
        }

        public UserRole UserLogin(string email, string password)
        {
            string query = "SELECT * FROM UserRole WHERE email = @email AND password = @password";
            using var cmd = dbHelper.GetNpgsqlCommand(query);
            cmd.Parameters.AddWithValue("email", email);
            cmd.Parameters.AddWithValue("password", password);
            using var reader = cmd.ExecuteReader();
            UserRole user = null;
            if (reader.Read())
            {
                user = new UserRole()
                {
                    Id_User = reader.GetInt32(reader.GetOrdinal("id_user")),
                    Nama_Lengkap = reader.GetString(reader.GetOrdinal("nama_lengkap")),
                    Email = reader.GetString(reader.GetOrdinal("email")),
                    Password = reader.GetString(reader.GetOrdinal("password")),
                    NIK = reader.GetInt64(reader.GetOrdinal("nik")),
                    No_Handphone = reader.GetInt64(reader.GetOrdinal("no_handphone")),
                    Foto_Profil = reader.IsDBNull(reader.GetOrdinal("foto_profil")) ? null : reader.GetString(reader.GetOrdinal("foto_profil"))
                };
            }
            dbHelper.closeConnection();
            return user;
        }

        public AdminRole AdminLogin(string email, string password)
        {
            string query = "SELECT * FROM AdminRole WHERE email = @email";
            using var cmd = dbHelper.GetNpgsqlCommand(query);
            cmd.Parameters.AddWithValue("email", email);
            using var reader = cmd.ExecuteReader();
            AdminRole admin = null;
            if (reader.Read())
            {
                // kalau mau password bisa dicek disini juga, karena tidak ada password di tabel admin (kalau nanti butuh, tinggal tambahkan)
                admin = new AdminRole()
                {
                    Id_Admin = reader.GetInt32(reader.GetOrdinal("id_admin")),
                    Nama = reader.GetString(reader.GetOrdinal("nama")),
                    Email = reader.GetString(reader.GetOrdinal("email")),
                    No_Handphone = reader.GetInt64(reader.GetOrdinal("no_handphone")),
                    Foto_Profil = reader.IsDBNull(reader.GetOrdinal("foto_profil")) ? null : reader.GetString(reader.GetOrdinal("foto_profil"))
                };
            }
            dbHelper.closeConnection();
            return admin;
        }
    }
}
