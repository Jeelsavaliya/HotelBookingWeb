using HotelBookingWeb.Models;
using HotelBookingWeb.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HotelBookingWeb.Controllers
{
    public class RoomTypeController : Controller
    {
        private readonly IRoomTypeService _roomTypeService;
        public RoomTypeController(IRoomTypeService roomTypeService)
        {
            _roomTypeService = roomTypeService;
        }


        public async Task<IActionResult> RoomTypeIndex()
        {
            List<RoomTypeDto>? list = new();

            ResponseDto? response = await _roomTypeService.GetAllRoomTypesAsync();

            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<RoomTypeDto>>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(list);
        }

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
                    return RedirectToAction(nameof(RoomTypeIndex));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }
            return View(model);
        }

        public async Task<IActionResult> RoomTypeDelete(int roomTypeId)
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
        public async Task<IActionResult> RoomTypeDelete(RoomTypeDto roomTypeDto)
        {
            ResponseDto? response = await _roomTypeService.DeleteRoomTypesAsync(roomTypeDto.RoomTypeID);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "RoomType deleted successfully";
                return RedirectToAction(nameof(RoomTypeIndex));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(roomTypeDto);
        }

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
                    return RedirectToAction(nameof(RoomTypeIndex));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }
            return View(roomTypeDto);
        }
    }
}
