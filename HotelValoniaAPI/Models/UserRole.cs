namespace HotelValoniaAPI.Models
{
    public class UserRole
    {
        public int Id_User { get; set; }           
        public string Nama_Lengkap { get; set; }     
        public string Email { get; set; }          
        public string Password { get; set; }       
        public long NIK { get; set; }           
        public long No_Handphone { get; set; }       
        public string? Foto_Profil { get; set; }        
    }
}
