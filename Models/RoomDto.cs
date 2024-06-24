using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBookingWeb.Models
{
    public class RoomDto
    {
        public int RoomID { get; set; }
        public int RoomTypeID { get; set; }
        
        public string Status { get; set; }
        public IFormFile? File { get; set; }
        public string? Image { get; set; } = String.Empty;
        public string Discription { get; set; }
        [Range(1, 5, ErrorMessage = "Please enter below 5 members")]
        public int Capacity { get; set; }
        public int RoomNumber { get; set; }
        [RegularExpression(@"^\$?([1-9]{1}[0-9]{0,2}(\,[0-9]{3})*(\.[0-9]{0,2})?|[1-9]{1}[0-9]{0,}(\.[0-9]{0,2})?|0(\.[0-9]{0,2})?|(\.[0-9]{1,2})?)$", ErrorMessage = "{0} must be in positive.")]
        public decimal PricePerNight { get; set; }
        /*[NotMapped]
        public RoomTypeDto? RoomType { get; set; }*/
    }
}
