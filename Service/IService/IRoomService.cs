using HotelBookingWeb.Models;

namespace HotelBookingWeb.Service.IService
{
    public interface IRoomService
    {
        Task<ResponseDto?> GetAllRoomAsync();
        Task<ResponseDto?> GetRoomByIdAsync(int id);
        Task<ResponseDto?> CreateRoomAsync(RoomDto roomDto);
        Task<ResponseDto?> UpdateRoomAsync(RoomDto roomDto);
        Task<ResponseDto?> DeleteRoomAsync(int id);

    }
}
