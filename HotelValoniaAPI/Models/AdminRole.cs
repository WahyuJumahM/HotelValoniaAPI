namespace HotelValoniaAPI.Models
{
    public class AdminRole
    {
        public int Id_Admin { get; set; }               // SERIAL PK
        public string Nama { get; set; }                // VARCHAR(100)
        public string Email { get; set; }               // VARCHAR(100) UNIQUE
        public string Password { get; set; }
        public long No_Handphone { get; set; }          // BIGINT
        public string? Foto_Profil { get; set; }        // VARCHAR(255), nullable
    }
}
