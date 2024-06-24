namespace HotelBookingWeb.Utility
{
    public class SD
    {
        public static string RoomTypeAPIBase { get; set; }
        public static string RoomAPIBase { get; set; }
        public static string AuthAPIBase { get; set; }
        public static string BookingRoomAPIBase { get; set; }
        public static string CheckAvailabilityAPIBase { get; set; }
        public static string EmialAPIBase { get; set; }
        public static string UserAPIBase { get; set; }



        public const string RoleAdmin = "ADMIN";
        public const string RoleCustomer = "CUSTOMER";
        public const string TokenCookie = "JWTToken";
        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE, 
            GETBYUSER
        }
        public enum ContentType
        {
            Json,
            MultipartFormData,
        }
    }
}
