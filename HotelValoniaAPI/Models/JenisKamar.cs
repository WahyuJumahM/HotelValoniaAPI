namespace HotelValoniaAPI.Models
{
    public class JenisKamar
    {
        public int Id_Jenis_Kamar { get; set; }        
        public string Deskripsi { get; set; }   
        public decimal Harga { get; set; }          
        public int Kapasitas { get; set; }                
        public string? Foto_Kamar { get; set; }    
        public int? Id_Tipe_Kasur { get; set; }          
    }
}
