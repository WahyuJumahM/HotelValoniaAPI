using System.Collections.Generic;
using Npgsql;
using HotelValoniaAPI.Models;
using HotelValoniaAPI.Helpers;

namespace HotelValoniaAPI.Context
{
    public class JenisKamarContext
    {
        private readonly sqlDBHelper dbHelper;

        public JenisKamarContext(string connectionString)
        {
            dbHelper = new sqlDBHelper(connectionString);
        }

        public bool Insert(JenisKamar jenisKamar)
        {
            string query = @"INSERT INTO Jenis_kamar 
                (deskripsi, harga, kapasitas, foto_kamar, id_tipe_kasur) 
                VALUES (@deskripsi, @harga, @kapasitas, @foto_kamar, @id_tipe_kasur)";
            using var cmd = dbHelper.GetNpgsqlCommand(query);
            cmd.Parameters.AddWithValue("deskripsi", jenisKamar.Deskripsi);
            cmd.Parameters.AddWithValue("harga", jenisKamar.Harga);
            cmd.Parameters.AddWithValue("kapasitas", jenisKamar.Kapasitas);
            cmd.Parameters.AddWithValue("foto_kamar", (object?)jenisKamar.Foto_Kamar ?? DBNull.Value);
            cmd.Parameters.AddWithValue("id_tipe_kasur", (object?)jenisKamar.Id_Tipe_Kasur ?? DBNull.Value);

            int affected = cmd.ExecuteNonQuery();
            dbHelper.closeConnection();
            return affected > 0;
        }

        public List<JenisKamar> GetAll()
        {
            var list = new List<JenisKamar>();
            string query = "SELECT * FROM Jenis_kamar ORDER BY id_jenis_kamar";
            using var cmd = dbHelper.GetNpgsqlCommand(query);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new JenisKamar()
                {
                    Id_Jenis_Kamar = reader.GetInt32(reader.GetOrdinal("id_jenis_kamar")),
                    Deskripsi = reader.GetString(reader.GetOrdinal("deskripsi")),
                    Harga = reader.GetDecimal(reader.GetOrdinal("harga")),
                    Kapasitas = reader.GetInt32(reader.GetOrdinal("kapasitas")),
                    Foto_Kamar = reader.IsDBNull(reader.GetOrdinal("foto_kamar")) ? null : reader.GetString(reader.GetOrdinal("foto_kamar")),
                    Id_Tipe_Kasur = reader.IsDBNull(reader.GetOrdinal("id_tipe_kasur")) ? null : reader.GetInt32(reader.GetOrdinal("id_tipe_kasur"))
                });
            }
            dbHelper.closeConnection();
            return list;
        }

        public bool Update(JenisKamar jenisKamar)
        {
            string query = @"UPDATE Jenis_kamar SET 
                deskripsi = @deskripsi, harga = @harga, kapasitas = @kapasitas, 
                foto_kamar = @foto_kamar, id_tipe_kasur = @id_tipe_kasur 
                WHERE id_jenis_kamar = @id";
            using var cmd = dbHelper.GetNpgsqlCommand(query);
            cmd.Parameters.AddWithValue("deskripsi", jenisKamar.Deskripsi);
            cmd.Parameters.AddWithValue("harga", jenisKamar.Harga);
            cmd.Parameters.AddWithValue("kapasitas", jenisKamar.Kapasitas);
            cmd.Parameters.AddWithValue("foto_kamar", (object?)jenisKamar.Foto_Kamar ?? DBNull.Value);
            cmd.Parameters.AddWithValue("id_tipe_kasur", (object?)jenisKamar.Id_Tipe_Kasur ?? DBNull.Value);
            cmd.Parameters.AddWithValue("id", jenisKamar.Id_Jenis_Kamar);

            int affected = cmd.ExecuteNonQuery();
            dbHelper.closeConnection();
            return affected > 0;
        }

        public bool Delete(int id)
        {
            string query = "DELETE FROM Jenis_kamar WHERE id_jenis_kamar = @id";
            using var cmd = dbHelper.GetNpgsqlCommand(query);
            cmd.Parameters.AddWithValue("id", id);
            int affected = cmd.ExecuteNonQuery();
            dbHelper.closeConnection();
            return affected > 0;
        }
    }
}
