using HotelBookingWeb.Models;

namespace HotelBookingWeb.Service.IService
{
    public interface ICheckAvailabilityService
    {
        Task<ResponseDto?> CreateCheckAvailabilityAsync(CheckAvailabilityDto checkAvailabilityDto);
        Task<ResponseDto?> DeleteCheckAvailabilityAsync(int id);

    }
}
