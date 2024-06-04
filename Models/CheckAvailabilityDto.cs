namespace HotelBookingWeb.Models
{
    public class CheckAvailabilityDto
    {
        public int CheckAvailabilityID { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime CheckOutTime { get; set; }
    }
}
