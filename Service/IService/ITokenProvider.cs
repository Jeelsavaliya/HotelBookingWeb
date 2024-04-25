using HotelBookingWeb.Models;

namespace HotelBookingWeb.Service.IService
{
    public interface ITokenProvider
    {
        void SetToken(string token);
        string? GetToken();
        void ClearToken();
    }
}
