using HotelBookingWeb.Models;
using HotelBookingWeb.Service.IService;
using HotelBookingWeb.Utility;

namespace HotelBookingWeb.Service
{
    public class CheckAvailabilityService : ICheckAvailabilityService
    {
        private readonly IBaseService _baseService;

        public CheckAvailabilityService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto?> CreateCheckAvailabilityAsync(CheckAvailabilityDto checkAvailabilityDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = checkAvailabilityDto,
                Url = SD.RoomAPIBase + "/api/CheckAvailabilityAPI",
                ContentType = SD.ContentType.MultipartFormData
            });
        }

        public async Task<ResponseDto?> DeleteCheckAvailabilityAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.DELETE,
                Url = SD.RoomAPIBase + "/api/CheckAvailabilityAPI/" + id
            });
        }

    }
}
