using System;
using Npgsql;
using HotelValoniaAPI.Helpers;
using HotelValoniaAPI.Models;

namespace HotelValoniaAPI.Context
{
    public class RegisterContext
    {
        private readonly sqlDBHelper dbHelper;

        public RegisterContext(string connectionString)
        {
            dbHelper = new sqlDBHelper(connectionString);
        }

        public bool RegisterUser(UserRole user)
        {
            string query = @"INSERT INTO UserRole 
                (nama_lengkap, email, password, NIK, no_handphone, foto_profil) 
                VALUES 
                (@nama_lengkap, @email, @password, @nik, @no_handphone, @foto_profil)";
            using var cmd = dbHelper.GetNpgsqlCommand(query);
            cmd.Parameters.AddWithValue("nama_lengkap", user.Nama_Lengkap);
            cmd.Parameters.AddWithValue("email", user.Email);
            cmd.Parameters.AddWithValue("password", user.Password);
            cmd.Parameters.AddWithValue("nik", user.NIK);
            cmd.Parameters.AddWithValue("no_handphone", user.No_Handphone);
            cmd.Parameters.AddWithValue("foto_profil", (object)user.Foto_Profil ?? DBNull.Value);

            int affected = cmd.ExecuteNonQuery();
            dbHelper.closeConnection();
            return affected > 0;
        }
    }
}
