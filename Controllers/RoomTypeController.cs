using HotelBookingWeb.Authentication;
using HotelBookingWeb.Models;
using HotelBookingWeb.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HotelBookingWeb.Controllers
{
    [CheckAccess]
    public class RoomTypeController : Controller
    {
        private readonly IRoomTypeService _roomTypeService;
        public RoomTypeController(IRoomTypeService roomTypeService)
        {
            _roomTypeService = roomTypeService;
        }

        #region Room Type Index
        public async Task<IActionResult> RoomTypeIndex()
        {
            List<RoomTypeDto>? list = new();

            ResponseDto? response = await _roomTypeService.GetAllRoomTypesAsync();

            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<RoomTypeDto>>(Convert.ToString(response.Result));
                if (list.Count() > 0)
                {
                    list.All(t =>
                    {
                        t.Image = "https://localhost:7001/" + t.Image;
                        return true;
                    });
                }
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(list);
        }
        #endregion

        #region Create Room Type
        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> RoomTypeCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RoomTypeCreate(RoomTypeDto model)
        {
            if (ModelState.IsValid)
            {
                ResponseDto? response = await _roomTypeService.CreateRoomTypesAsync(model);

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "RoomType created successfully";
                    return RedirectToAction("RoomTypeIndex");
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }
            return View(model);
        }
        #endregion

        #region Delete Room Type
        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> RoomTypeDelete(int roomTypeId)
        {

            //Delete Type on Index Page (not rendering delete view page)
            ResponseDto? response = await _roomTypeService.DeleteRoomTypesAsync(roomTypeId);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "RoomType deleted successfully";
                return RedirectToAction("RoomTypeIndex");
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            /*return View(roomTypeDto);*/


            //Delete Type on selecte ID and go perticuler delete page
            /*ResponseDto? response = await _roomTypeService.GetRoomTypeByIdAsync(roomTypeId);

            if (response != null && response.IsSuccess)
            {
                RoomTypeDto? model = JsonConvert.DeserializeObject<RoomTypeDto>(Convert.ToString(response.Result));
                return View(model);
            }
            else
            {
                TempData["error"] = response?.Message;
            }*/
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> RoomTypeDelete(RoomTypeDto roomTypeDto)
        {
            ResponseDto? response = await _roomTypeService.DeleteRoomTypesAsync(roomTypeDto.RoomTypeID);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "RoomType deleted successfully";
                return RedirectToAction("RoomTypeIndex");
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(roomTypeDto);
        }
        #endregion

        #region Update Room Type
        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> RoomTypeEdit(int roomTypeId)
        {
            ResponseDto? response = await _roomTypeService.GetRoomTypeByIdAsync(roomTypeId);

            if (response != null && response.IsSuccess)
            {
                RoomTypeDto? model = JsonConvert.DeserializeObject<RoomTypeDto>(Convert.ToString(response.Result));
                return View(model);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> RoomTypeEdit(RoomTypeDto roomTypeDto)
            {
            if (ModelState.IsValid)
            {
                ResponseDto? response = await _roomTypeService.UpdateRoomTypesAsync(roomTypeDto);

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "RoomType updated successfully";
                    return RedirectToAction("RoomTypeIndex");
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }
            return View(roomTypeDto);
        }
        #endregion
    }
}
