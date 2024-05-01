using HotelBookingWeb.Models;

namespace HotelBookingWeb.Service.IService
{
    public interface IRoomTypeService
    {
        Task<ResponseDto?> GetAllRoomTypesAsync();
        Task<ResponseDto?> GetRoomTypeByIdAsync(int id);
        Task<ResponseDto?> CreateRoomTypesAsync(RoomTypeDto roomTypeDto);
        Task<ResponseDto?> UpdateRoomTypesAsync(RoomTypeDto roomTypeDto);
        Task<ResponseDto?> DeleteRoomTypesAsync(int id);
    }
}
