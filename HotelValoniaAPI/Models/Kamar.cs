namespace HotelValoniaAPI.Models
{
    public class Kamar
    {
        public int Id_Kamar { get; set; }          
        public string Nama_Kamar { get; set; }          
        public int Lantai { get; set; }         
        public int Stok { get; set; }                  
        public int? Id_Jenis_Kamar { get; set; }     
    }
}
