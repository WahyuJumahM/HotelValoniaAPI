using System.Collections.Generic;
using Npgsql;
using HotelValoniaAPI.Models;
using HotelValoniaAPI.Helpers;

namespace HotelValoniaAPI.Context
{
    public class StatusContext
    {
        private readonly sqlDBHelper dbHelper;

        public StatusContext(string connectionString)
        {
            dbHelper = new sqlDBHelper(connectionString);
        }

        public bool Insert(Status status)
        {
            string query = "INSERT INTO Status (nama_status) VALUES (@nama_status)";
            using var cmd = dbHelper.GetNpgsqlCommand(query);
            cmd.Parameters.AddWithValue("nama_status", status.Nama_Status);
            int affected = cmd.ExecuteNonQuery();
            dbHelper.closeConnection();
            return affected > 0;
        }

        public List<Status> GetAll()
        {
            var list = new List<Status>();
            string query = "SELECT * FROM Status ORDER BY id_status";
            using var cmd = dbHelper.GetNpgsqlCommand(query);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Status()
                {
                    Id_Status = reader.GetInt32(reader.GetOrdinal("id_status")),
                    Nama_Status = reader.GetString(reader.GetOrdinal("nama_status"))
                });
            }
            dbHelper.closeConnection();
            return list;
        }

        public bool Update(Status status)
        {
            string query = "UPDATE Status SET nama_status = @nama_status WHERE id_status = @id";
            using var cmd = dbHelper.GetNpgsqlCommand(query);
            cmd.Parameters.AddWithValue("nama_status", status.Nama_Status);
            cmd.Parameters.AddWithValue("id", status.Id_Status);
            int affected = cmd.ExecuteNonQuery();
            dbHelper.closeConnection();
            return affected > 0;
        }

        public bool Delete(int id)
        {
            string query = "DELETE FROM Status WHERE id_status = @id";
            using var cmd = dbHelper.GetNpgsqlCommand(query);
            cmd.Parameters.AddWithValue("id", id);
            int affected = cmd.ExecuteNonQuery();
            dbHelper.closeConnection();
            return affected > 0;
        }
    }
}
