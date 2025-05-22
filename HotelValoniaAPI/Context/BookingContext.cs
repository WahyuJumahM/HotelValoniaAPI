using System.Collections.Generic;
using Npgsql;
using HotelValoniaAPI.Models;
using HotelValoniaAPI.Helpers;

namespace HotelValoniaAPI.Context
{
    public class BookingContext
    {
        private readonly sqlDBHelper dbHelper;

        public BookingContext(string connectionString)
        {
            dbHelper = new sqlDBHelper(connectionString);
        }

        public bool Insert(Booking booking)
        {
            string query = @"INSERT INTO Booking 
                (catatan, cek_in, cek_out, id_user, id_admin, id_kamar, id_status)
                VALUES (@catatan, @cek_in, @cek_out, @id_user, @id_admin, @id_kamar, @id_status)";
            using var cmd = dbHelper.GetNpgsqlCommand(query);
            cmd.Parameters.AddWithValue("catatan", booking.Catatan ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("cek_in", booking.Cek_In);
            cmd.Parameters.AddWithValue("cek_out", booking.Cek_Out);
            cmd.Parameters.AddWithValue("id_user", booking.Id_User);
            cmd.Parameters.AddWithValue("id_admin", booking.Id_Admin);
            cmd.Parameters.AddWithValue("id_kamar", booking.Id_Kamar);
            cmd.Parameters.AddWithValue("id_status", booking.Id_Status);
            int affected = cmd.ExecuteNonQuery();
            dbHelper.closeConnection();
            return affected > 0;
        }

        public List<Booking> GetAll()
        {
            var list = new List<Booking>();
            string query = "SELECT * FROM Booking ORDER BY id_booking";
            using var cmd = dbHelper.GetNpgsqlCommand(query);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Booking()
                {
                    Id_Booking = reader.GetInt32(reader.GetOrdinal("id_booking")),
                    Catatan = reader.IsDBNull(reader.GetOrdinal("catatan")) ? null : reader.GetString(reader.GetOrdinal("catatan")),
                    Cek_In = reader.GetDateTime(reader.GetOrdinal("cek_in")),
                    Cek_Out = reader.GetDateTime(reader.GetOrdinal("cek_out")),
                    Id_User = reader.GetInt32(reader.GetOrdinal("id_user")),
                    Id_Admin = reader.GetInt32(reader.GetOrdinal("id_admin")),
                    Id_Kamar = reader.GetInt32(reader.GetOrdinal("id_kamar")),
                    Id_Status = reader.GetInt32(reader.GetOrdinal("id_status"))
                });
            }
            dbHelper.closeConnection();
            return list;
        }

        public bool Delete(int id)
        {
            string query = "DELETE FROM Booking WHERE id_booking = @id";
            using var cmd = dbHelper.GetNpgsqlCommand(query);
            cmd.Parameters.AddWithValue("id", id);
            int affected = cmd.ExecuteNonQuery();
            dbHelper.closeConnection();
            return affected > 0;
        }
    }
}
