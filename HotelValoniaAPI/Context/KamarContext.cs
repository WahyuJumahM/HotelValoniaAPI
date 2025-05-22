using System.Collections.Generic;
using Npgsql;
using HotelValoniaAPI.Models;
using HotelValoniaAPI.Helpers;

namespace HotelValoniaAPI.Context
{
    public class KamarContext
    {
        private readonly sqlDBHelper dbHelper;

        public KamarContext(string connectionString)
        {
            dbHelper = new sqlDBHelper(connectionString);
        }

        public bool Insert(Kamar kamar)
        {
            string query = @"INSERT INTO Kamar (nama_kamar, lantai, stok, id_jenis_kamar) 
                             VALUES (@nama_kamar, @lantai, @stok, @id_jenis_kamar)";
            using var cmd = dbHelper.GetNpgsqlCommand(query);
            cmd.Parameters.AddWithValue("nama_kamar", kamar.Nama_Kamar);
            cmd.Parameters.AddWithValue("lantai", kamar.Lantai);
            cmd.Parameters.AddWithValue("stok", kamar.Stok);
            cmd.Parameters.AddWithValue("id_jenis_kamar", kamar.Id_Jenis_Kamar);
            int affected = cmd.ExecuteNonQuery();
            dbHelper.closeConnection();
            return affected > 0;
        }

        public List<Kamar> GetAll()
        {
            var list = new List<Kamar>();
            string query = "SELECT * FROM Kamar ORDER BY id_kamar";
            using var cmd = dbHelper.GetNpgsqlCommand(query);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Kamar()
                {
                    Id_Kamar = reader.GetInt32(reader.GetOrdinal("id_kamar")),
                    Nama_Kamar = reader.GetString(reader.GetOrdinal("nama_kamar")),
                    Lantai = reader.GetInt32(reader.GetOrdinal("lantai")),
                    Stok = reader.GetInt32(reader.GetOrdinal("stok")),
                    Id_Jenis_Kamar = reader.GetInt32(reader.GetOrdinal("id_jenis_kamar"))
                });
            }
            dbHelper.closeConnection();
            return list;
        }

        public bool Update(Kamar kamar)
        {
            string query = @"UPDATE Kamar SET 
                             nama_kamar = @nama_kamar, lantai = @lantai, stok = @stok, id_jenis_kamar = @id_jenis_kamar 
                             WHERE id_kamar = @id";
            using var cmd = dbHelper.GetNpgsqlCommand(query);
            cmd.Parameters.AddWithValue("nama_kamar", kamar.Nama_Kamar);
            cmd.Parameters.AddWithValue("lantai", kamar.Lantai);
            cmd.Parameters.AddWithValue("stok", kamar.Stok);
            cmd.Parameters.AddWithValue("id_jenis_kamar", kamar.Id_Jenis_Kamar);
            cmd.Parameters.AddWithValue("id", kamar.Id_Kamar);
            int affected = cmd.ExecuteNonQuery();
            dbHelper.closeConnection();
            return affected > 0;
        }

        public bool Delete(int id)
        {
            string query = "DELETE FROM Kamar WHERE id_kamar = @id";
            using var cmd = dbHelper.GetNpgsqlCommand(query);
            cmd.Parameters.AddWithValue("id", id);
            int affected = cmd.ExecuteNonQuery();
            dbHelper.closeConnection();
            return affected > 0;
        }
    }
}
