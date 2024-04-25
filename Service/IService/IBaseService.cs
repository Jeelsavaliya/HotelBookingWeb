using HotelBookingWeb.Models;

namespace HotelBookingWeb.Service.IService
{
    public interface IBaseService
    {
        Task<ResponseDto?> SendAsync(RequestDto requestDto, bool withBearer = true);
    }
}
