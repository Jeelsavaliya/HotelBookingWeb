using HotelBookingWeb.Models;
using HotelBookingWeb.Service.IService;
using HotelBookingWeb.Utility;

namespace HotelBookingWeb.Service
{
    public class RoomTypeService : IRoomTypeService
    {
        private readonly IBaseService _baseService;

        public RoomTypeService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto?> CreateRoomTypesAsync(RoomTypeDto roomTypeDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = roomTypeDto,
                Url = SD.RoomTypeAPIBase + "/api/RoomTypeAPI",
                ContentType = SD.ContentType.MultipartFormData
            });
        }
        
        public async Task<ResponseDto?> DeleteRoomTypesAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.DELETE,
                Url = SD.RoomTypeAPIBase + "/api/RoomTypeAPI/" + id
            });
        }

        public async Task<ResponseDto?> GetAllRoomTypesAsync()
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.RoomTypeAPIBase + "/api/RoomTypeAPI"
            });
        }

        public async Task<ResponseDto?> GetRoomTypeByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.RoomTypeAPIBase + "/api/RoomTypeAPI/" + id
            });
        }

        public async Task<ResponseDto?> UpdateRoomTypesAsync(RoomTypeDto roomTypeDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.PUT,
                Data = roomTypeDto,
                Url = SD.RoomTypeAPIBase + $"/api/RoomTypeAPI/{roomTypeDto.RoomTypeID} ",
                ContentType = SD.ContentType.MultipartFormData
            });
        }
    }
}
