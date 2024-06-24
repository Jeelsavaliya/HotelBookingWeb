using HotelBookingWeb.Models;
using HotelBookingWeb.Service.IService;
using HotelBookingWeb.Utility;

namespace HotelBookingWeb.Service
{
    public class RoomService : IRoomService
    {
        private readonly IBaseService _baseService;

        public RoomService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto?> CreateRoomAsync(RoomDto roomDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = roomDto,
                Url = SD.RoomAPIBase + "/api/RoomAPI",
                ContentType = SD.ContentType.MultipartFormData
            });
        }

        public async Task<ResponseDto?> DeleteRoomAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.DELETE,
                Url = SD.RoomAPIBase + "/api/RoomAPI/" + id
            });
        }

        public async Task<ResponseDto?> GetAllRoomAsync()
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.RoomAPIBase + "/api/RoomAPI"
            });
        }
        public async Task<ResponseDto?> GetRoomByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.RoomAPIBase + "/api/RoomAPI/" + id
            });
        }

        public async Task<ResponseDto?> UpdateRoomAsync(RoomDto roomDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.PUT,
                Data = roomDto,
                Url = SD.RoomAPIBase + "/api/RoomAPI",
                ContentType = SD.ContentType.MultipartFormData
            });
        }


    }
}
