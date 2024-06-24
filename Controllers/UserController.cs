using HotelBookingWeb.Models;
using HotelBookingWeb.Service;
using HotelBookingWeb.Service.IService;
using HotelBookingWeb.Utility;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;

namespace HotelBookingWeb.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        #region User Index
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> UserIndex()
        {
            List<UserDto>? list = new();

            ResponseDto? response = await _userService.GetAllUserAsync();

            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<UserDto>>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(list);
        }
        #endregion

        #region UserProfile
        public async Task<IActionResult> UserProfile()
        {
            string userId = "";
            if (User.IsInRole(SD.RoleCustomer))
            {
                userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            }


            ResponseDto? response = await _userService.GetUserByIdAsync(userId);

            if (response != null && response.IsSuccess)
            {
                UserDto model = JsonConvert.DeserializeObject<UserDto>(Convert.ToString(response.Result));
                //var realpass = new PasswordHasher().UnHashPassword(model.PasswordHash);
                
                return View(model);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View();
        }

        #endregion

        #region Update User Profile
        [HttpGet]
        public async Task<IActionResult> UserEdit()
        {
            string userId = "";
            if (User.IsInRole(SD.RoleCustomer))
            {
                userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            }

            //Update Data by ID
            ResponseDto? response = await _userService.GetUserByIdAsync(userId);

            if (response != null && response.IsSuccess)
            {
                UserDto? model = JsonConvert.DeserializeObject<UserDto>(Convert.ToString(response.Result));
                //ApplicationUserDto model = JsonConvert.DeserializeObject<UserDto>(Convert.ToString(response.Result));
                return View(model);
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> UserEdit(UserDto applicationuserDto)
        {
            if (ModelState.IsValid)
            {
                ResponseDto? response = await _userService.UpdateUserByUserIdAsync(applicationuserDto);

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "User updated successfully";
                    return RedirectToAction("UserIndex");
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }
            return View(applicationuserDto);
            }
        #endregion
    }
}
