using System.Collections.Generic;
using Npgsql;
using HotelValoniaAPI.Models;
using HotelValoniaAPI.Helpers;

namespace HotelValoniaAPI.Context
{
    public class TipeKasurContext
    {
        private readonly sqlDBHelper dbHelper;

        public TipeKasurContext(string connectionString)
        {
            dbHelper = new sqlDBHelper(connectionString);
        }

        public bool Insert(TipeKasur tipe)
        {
            string query = "INSERT INTO Tipe_kasur (tipe_kasur) VALUES (@tipe_kasur)";
            using var cmd = dbHelper.GetNpgsqlCommand(query);
            cmd.Parameters.AddWithValue("tipe_kasur", tipe.Tipe_Kasur);
            int affected = cmd.ExecuteNonQuery();
            dbHelper.closeConnection();
            return affected > 0;
        }

        public List<TipeKasur> GetAll()
        {
            var list = new List<TipeKasur>();
            string query = "SELECT * FROM Tipe_kasur ORDER BY id_tipe_kasur";
            using var cmd = dbHelper.GetNpgsqlCommand(query);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new TipeKasur()
                {
                    Id_Tipe_Kasur = reader.GetInt32(reader.GetOrdinal("id_tipe_kasur")),
                    Tipe_Kasur = reader.GetString(reader.GetOrdinal("tipe_kasur"))
                });
            }
            dbHelper.closeConnection();
            return list;
        }

        public bool Update(TipeKasur tipe)
        {
            string query = "UPDATE Tipe_kasur SET tipe_kasur = @tipe_kasur WHERE id_tipe_kasur = @id";
            using var cmd = dbHelper.GetNpgsqlCommand(query);
            cmd.Parameters.AddWithValue("tipe_kasur", tipe.Tipe_Kasur);
            cmd.Parameters.AddWithValue("id", tipe.Id_Tipe_Kasur);
            int affected = cmd.ExecuteNonQuery();
            dbHelper.closeConnection();
            return affected > 0;
        }

        public bool Delete(int id)
        {
            string query = "DELETE FROM Tipe_kasur WHERE id_tipe_kasur = @id";
            using var cmd = dbHelper.GetNpgsqlCommand(query);
            cmd.Parameters.AddWithValue("id", id);
            int affected = cmd.ExecuteNonQuery();
            dbHelper.closeConnection();
            return affected > 0;
        }
    }
}
