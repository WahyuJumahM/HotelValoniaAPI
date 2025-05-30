using System.Collections.Generic;
using System.Data;
using Npgsql;
using HotelValoniaAPI.Models;
using HotelValoniaAPI.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace HotelValoniaAPI.Context
{
    public class UserRoleContext
    {
        private readonly sqlDBHelper dbHelper;

        public UserRoleContext(string connectionString)
        {
            dbHelper = new sqlDBHelper(connectionString);
        }

        // CREATE UserRole
        public bool InsertUser(UserRole user)
        {
            string query = @"INSERT INTO UserRole (nama_lengkap, email, password, NIK, no_handphone, foto_profil) 
                             VALUES (@nama_lengkap, @email, @password, @nik, @no_handphone, @foto_profil)";
            using var cmd = dbHelper.GetNpgsqlCommand(query);

            cmd.Parameters.AddWithValue("nama_lengkap", user.Nama_Lengkap);
            cmd.Parameters.AddWithValue("email", user.Email);
            cmd.Parameters.AddWithValue("password", user.Password);
            cmd.Parameters.AddWithValue("nik", user.NIK);
            cmd.Parameters.AddWithValue("no_handphone", user.No_Handphone);
            cmd.Parameters.AddWithValue("foto_profil", (object?)user.Foto_Profil ?? DBNull.Value);

            int affectedRows = cmd.ExecuteNonQuery();
            dbHelper.closeConnection();
            return affectedRows > 0;
        }

        // READ UserRole
        public List<UserRole> GetAllUsers(string role, int idUser)
        {
            List<UserRole> listUsers = new List<UserRole>();
            string query;

            if (role == "Admin")
            {
                query = "SELECT * FROM UserRole ORDER BY id_user";
            }
            else
            {
                query = "SELECT * FROM UserRole WHERE id_user = @id_user";
            }

            using var cmd = dbHelper.GetNpgsqlCommand(query);

            if (role != "Admin")
            {
                cmd.Parameters.AddWithValue("id_user", idUser);
            }

            using NpgsqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                UserRole user = new UserRole()
                {
                    Id_User = reader.GetInt32(reader.GetOrdinal("id_user")),
                    Nama_Lengkap = reader.GetString(reader.GetOrdinal("nama_lengkap")),
                    Email = reader.GetString(reader.GetOrdinal("email")),
                    Password = reader.GetString(reader.GetOrdinal("password")),
                    NIK = reader.GetInt64(reader.GetOrdinal("nik")),
                    No_Handphone = reader.GetInt64(reader.GetOrdinal("no_handphone")),
                    Foto_Profil = reader.IsDBNull(reader.GetOrdinal("foto_profil")) ? null : reader.GetString(reader.GetOrdinal("foto_profil"))
                };
                listUsers.Add(user);
            }
            dbHelper.closeConnection();
            return listUsers;
        }




        // READ by ID
        public UserRole? GetUserById(int id)
        {
            UserRole? user = null;
            string query = "SELECT * FROM UserRole WHERE id_user = @id_user";

            using var cmd = dbHelper.GetNpgsqlCommand(query);
            cmd.Parameters.AddWithValue("id_user", id);

            using var reader = cmd.ExecuteReader();
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

        // UPDATE UserRole
        public bool UpdateUser(UserRole user)
        {
            string query = @"UPDATE UserRole SET 
                             nama_lengkap = @nama_lengkap,
                             email = @email,
                             password = @password,
                             nik = @nik,
                             no_handphone = @no_handphone,
                             foto_profil = @foto_profil
                             WHERE id_user = @id_user";

            using var cmd = dbHelper.GetNpgsqlCommand(query);

            cmd.Parameters.AddWithValue("nama_lengkap", user.Nama_Lengkap);
            cmd.Parameters.AddWithValue("email", user.Email);
            cmd.Parameters.AddWithValue("password", user.Password);
            cmd.Parameters.AddWithValue("nik", user.NIK);
            cmd.Parameters.AddWithValue("no_handphone", user.No_Handphone);
            cmd.Parameters.AddWithValue("foto_profil", (object?)user.Foto_Profil ?? DBNull.Value);
            cmd.Parameters.AddWithValue("id_user", user.Id_User);

            int affectedRows = cmd.ExecuteNonQuery();
            dbHelper.closeConnection();
            return affectedRows > 0;
        }

        // DELETE UserRole
        public bool DeleteUser(int id)
        {
            string query = "DELETE FROM UserRole WHERE id_user = @id_user";

            using var cmd = dbHelper.GetNpgsqlCommand(query);
            cmd.Parameters.AddWithValue("id_user", id);

            int affectedRows = cmd.ExecuteNonQuery();
            dbHelper.closeConnection();
            return affectedRows > 0;
        }

        internal ActionResult<List<UserRole>> GetAll()
        {
            throw new NotImplementedException();
        }

        internal ActionResult<UserRole> GetById(int id)
        {
            throw new NotImplementedException();
        }

        internal bool Insert(UserRole user)
        {
            throw new NotImplementedException();
        }

        internal bool Update(UserRole user)
        {
            throw new NotImplementedException();
        }

        internal bool Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
