namespace HotelBookingWeb.Models
{
    public class RoomTypeDto
    {
        public int RoomTypeID { get; set; }
        public string Name { get; set; }
        /*[NotMapped]*/
        public IFormFile? File { get; set; }
        public string? Photo { get; set; }
        public string Discription { get; set; }
        public string Services { get; set; }
        public decimal Size { get; set; }
        public int Capacity { get; set; }
    }
}
