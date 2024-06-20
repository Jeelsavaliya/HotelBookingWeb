using HotelBookingWeb.Models;

namespace HotelBookingWeb.Service.IService
{
    public interface IUserService
    {
        Task<ResponseDto?> GetAllUserAsync();
        Task<ResponseDto?> GetUserByIdAsync(string userId);
        Task<ResponseDto?> UpdateUserByEmailAsync(ApplicationUserDto applicationUserDto);  //For forgot passowrd
        Task<ResponseDto?> UpdateUserByUserIdAsync(UserDto UserDto); //For 
        Task<ResponseDto?> DeleteUserAsync(string userId);
        Task<ResponseDto?> GetUserByEmailAsync(string email);
    }
}
