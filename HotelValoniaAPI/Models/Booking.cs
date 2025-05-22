using System;

namespace HotelValoniaAPI.Models
{
    public class Booking
    {
        public int Id_Booking { get; set; }              
        public string? Catatan { get; set; }                
        public DateTime Cek_In { get; set; }              
        public DateTime Cek_Out { get; set; }   
        public int? Id_User { get; set; }               
        public int? Id_Admin { get; set; }            
        public int? Id_Kamar { get; set; }             
        public int? Id_Status { get; set; }              
    }
}
