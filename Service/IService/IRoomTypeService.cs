using HotelBookingWeb.Models;

namespace HotelBookingWeb.Service.IService
{
    public interface IRoomTypeService
    {
        Task<ResponseDto?> GetAllRoomTypesAsync();
        Task<ResponseDto?> GetRoomTypeByIdAsync(int id);
        Task<ResponseDto?> CreateRoomTypesAsync(RoomTypeDto productDto);
        Task<ResponseDto?> UpdateRoomTypesAsync(RoomTypeDto productDto);
        Task<ResponseDto?> DeleteRoomTypesAsync(int id);
    }
}
