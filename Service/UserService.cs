using HotelBookingWeb.Models;
using HotelBookingWeb.Service.IService;
using HotelBookingWeb.Utility;

namespace HotelBookingWeb.Service
{
    public class UserService : IUserService
    {
        private readonly IBaseService _baseService;

        public UserService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto?> DeleteUserAsync(string userId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.DELETE,
                Url = SD.UserAPIBase + "/api/UserAPI/" +userId
            });
        }

        public async Task<ResponseDto?> GetAllUserAsync()
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.UserAPIBase + "/api/UserAPI"
            });
        }

        public async Task<ResponseDto?> GetUserByEmailAsync(string email)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.UserAPIBase + "/api/UserAPI/" + email
            });
        }

        public async Task<ResponseDto?> GetUserByIdAsync(string userId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GETBYUSER,
                Url = SD.UserAPIBase + "/api/UserAPI/User/" + userId
            });
        }

        public async Task<ResponseDto?> UpdateUserByEmailAsync(ApplicationUserDto applicationUserDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.PUT,
                Data = applicationUserDto,
                Url = SD.UserAPIBase + "/api/UserAPI"
            });
        }

        public async Task<ResponseDto?> UpdateUserByUserIdAsync(UserDto UserDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.PUT,
                Data = UserDto,
                Url = SD.UserAPIBase + "/api/UserAPI"
            });
        }
    }
}
