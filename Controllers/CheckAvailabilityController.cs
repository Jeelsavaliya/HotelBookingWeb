using HotelBookingWeb.Models;
using HotelBookingWeb.Service;
using HotelBookingWeb.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;

namespace HotelBookingWeb.Controllers
{
    public class CheckAvailabilityController : Controller
    {
        private readonly IRoomService _roomService;
        private readonly ICheckAvailabilityService _checkAvailabilityService;
        private readonly IConfiguration _configuration;

        public CheckAvailabilityController(ICheckAvailabilityService checkAvailabilityService, IRoomService roomService, IConfiguration configuration)
        {
            _checkAvailabilityService = checkAvailabilityService;
            _roomService = roomService;
            _configuration = configuration;
        }

        #region Checking Rooms
        [HttpPost]
        public async Task<IActionResult> CheckRooms(CheckAvailabilityDto model)
        {
            if (ModelState.IsValid)
            {
                ResponseDto? response = await _checkAvailabilityService.CreateCheckAvailabilityAsync(model);

                if (response != null && response.IsSuccess)
                {
                   
                    List<RoomDto>? list = new();

                    ResponseDto? response1 = await _roomService.GetAllRoomAsync();

                    if (response1 != null && response1.IsSuccess)
                    {
                        list = JsonConvert.DeserializeObject<List<RoomDto>>(Convert.ToString(response1.Result));

                        var result = list.Where(x => x.Status == "Unoccupied").ToList();

                        if (result.Count() > 0)
                        {
                            result.All(t =>
                            {
                                var url = _configuration.GetSection("BaseUrl:WebUrl").Value;
                                t.Image = url + t.Image;
                                return true;
                            });
                        }
                      
                        return View(result);
                    }
                    else
                    {
                        TempData["error"] = response?.Message;
                    }
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }
        #endregion
    }
}
