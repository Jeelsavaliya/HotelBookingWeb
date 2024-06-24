using HotelBookingWeb.Models;

namespace HotelBookingWeb.Service.IService
{
    public interface IBookingRoomService
    {
        Task<ResponseDto?> GetAllBookingRoomAsync();
        Task<ResponseDto?> GetBookingRoomByIdAsync(int id);
        Task<ResponseDto?> CreateBookingRoomAsync(BookingRoomDto bookingRoomDto);
        Task<ResponseDto?> UpdateBookingRoomAsync(BookingRoomDto bookingRoomDto);
        Task<ResponseDto?> DeleteBookingRoomAsync(int id);

        Task<ResponseDto?> GetBookingRoomByUserIdAsync(string userid);
        /*Task<ResponseDto?> CreateStripeSession(StripeRequestDto stripeRequestDto);*/
    }
}
