using HotelBookingWeb.Models;
using HotelBookingWeb.Service.IService;
using HotelBookingWeb.Utility;

namespace HotelBookingWeb.Service
{
    public class BookingRoomService : IBookingRoomService
    {
        private readonly IBaseService _baseService;

        public BookingRoomService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto?> CreateBookingRoomAsync(BookingRoomDto bookingRoomDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = bookingRoomDto,
                Url = SD.BookingRoomAPIBase + "/api/BookingRoomAPI",
                //ContentType = SD.ContentType.MultipartFormData
            });
        }

        /*public async Task<ResponseDto?> CreateStripeSession(StripeRequestDto stripeRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = stripeRequestDto,
                Url = SD.BookingRoomAPIBase + "/api/BookingRoomAPI/CreateStripeSession"
            });
        }*/

        public async Task<ResponseDto?> DeleteBookingRoomAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.DELETE,
                Url = SD.BookingRoomAPIBase + "/api/BookingRoomAPI/" + id
            });
        }

        public async Task<ResponseDto?> GetAllBookingRoomAsync()
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.BookingRoomAPIBase + "/api/BookingRoomAPI"
            });
        }

        public async Task<ResponseDto?> GetBookingRoomByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.BookingRoomAPIBase + "/api/BookingRoomAPI/" + id
            });
        }

        public async Task<ResponseDto?> GetBookingRoomByUserIdAsync(string userid)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GETBYUSER,
                Url = SD.BookingRoomAPIBase + "/api/BookingRoomAPI/" + userid
            });
        }

        public async Task<ResponseDto?> UpdateBookingRoomAsync(BookingRoomDto bookingRoomDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.PUT,
                Data = bookingRoomDto,
                Url = SD.BookingRoomAPIBase + "/api/BookingRoomAPI",
                //ContentType = SD.ContentType.MultipartFormData
            });
        }
    }
}
