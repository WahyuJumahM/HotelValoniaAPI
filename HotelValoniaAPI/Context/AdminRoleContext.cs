using System.Collections.Generic;
using System.Data;
using Npgsql;
using HotelValoniaAPI.Models;
using HotelValoniaAPI.Helpers;

namespace HotelValoniaAPI.Context
{
    public class AdminRoleContext
    {
        private readonly sqlDBHelper dbHelper;

        public AdminRoleContext(string connectionString)
        {
            dbHelper = new sqlDBHelper(connectionString);
        }

        public bool InsertAdmin(AdminRole admin)
        {
            string query = @"INSERT INTO AdminRole (nama, email, no_handphone, foto_profil)
                             VALUES (@nama, @email, @no_handphone, @foto_profil)";
            using var cmd = dbHelper.GetNpgsqlCommand(query);
            cmd.Parameters.AddWithValue("nama", admin.Nama);
            cmd.Parameters.AddWithValue("email", admin.Email);
            cmd.Parameters.AddWithValue("no_handphone", admin.No_Handphone);
            cmd.Parameters.AddWithValue("foto_profil", (object?)admin.Foto_Profil ?? DBNull.Value);

            int affected = cmd.ExecuteNonQuery();
            dbHelper.closeConnection();
            return affected > 0;
        }

        public List<AdminRole> GetAllAdmins()
        {
            List<AdminRole> list = new List<AdminRole>();
            string query = "SELECT * FROM AdminRole ORDER BY id_admin";
            using var cmd = dbHelper.GetNpgsqlCommand(query);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new AdminRole()
                {
                    Id_Admin = reader.GetInt32(reader.GetOrdinal("id_admin")),
                    Nama = reader.GetString(reader.GetOrdinal("nama")),
                    Email = reader.GetString(reader.GetOrdinal("email")),
                    No_Handphone = reader.GetInt64(reader.GetOrdinal("no_handphone")),
                    Foto_Profil = reader.IsDBNull(reader.GetOrdinal("foto_profil")) ? null : reader.GetString(reader.GetOrdinal("foto_profil"))
                });
            }
            dbHelper.closeConnection();
            return list;
        }

        public AdminRole? GetAdminById(int id)
        {
            AdminRole? admin = null;
            string query = "SELECT * FROM AdminRole WHERE id_admin = @id_admin";
            using var cmd = dbHelper.GetNpgsqlCommand(query);
            cmd.Parameters.AddWithValue("id_admin", id);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
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

        public bool UpdateAdmin(AdminRole admin)
        {
            string query = @"UPDATE AdminRole SET 
                             nama = @nama,
                             email = @email,
                             no_handphone = @no_handphone,
                             foto_profil = @foto_profil
                             WHERE id_admin = @id_admin";
            using var cmd = dbHelper.GetNpgsqlCommand(query);
            cmd.Parameters.AddWithValue("nama", admin.Nama);
            cmd.Parameters.AddWithValue("email", admin.Email);
            cmd.Parameters.AddWithValue("no_handphone", admin.No_Handphone);
            cmd.Parameters.AddWithValue("foto_profil", (object?)admin.Foto_Profil ?? DBNull.Value);
            cmd.Parameters.AddWithValue("id_admin", admin.Id_Admin);

            int affected = cmd.ExecuteNonQuery();
            dbHelper.closeConnection();
            return affected > 0;
        }

        public bool DeleteAdmin(int id)
        {
            string query = "DELETE FROM AdminRole WHERE id_admin = @id_admin";
            using var cmd = dbHelper.GetNpgsqlCommand(query);
            cmd.Parameters.AddWithValue("id_admin", id);

            int affected = cmd.ExecuteNonQuery();
            dbHelper.closeConnection();
            return affected > 0;
        }
    }
}
