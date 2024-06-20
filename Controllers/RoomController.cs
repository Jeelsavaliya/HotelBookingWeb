using HotelBookingWeb.Authentication;
using HotelBookingWeb.Models;
using HotelBookingWeb.Service;
using HotelBookingWeb.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Data;

namespace HotelBookingWeb.Controllers
{
    [CheckAccess]
    public class RoomController : Controller
    {
        private readonly IRoomService _roomService;
        private readonly IRoomTypeService _roomTypeService;
        private readonly IConfiguration _configuration;

        public RoomController(IRoomService roomService,IRoomTypeService roomTypeService, IConfiguration configuration)
        {
            _roomService = roomService;
            _roomTypeService = roomTypeService;
            _configuration = configuration;
        }

        #region Room Index

        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> RoomIndex()
        {
            List<RoomDto>? list = new();

            ResponseDto? response = await _roomService.GetAllRoomAsync();

            if (response != null && response.IsSuccess)
            {
                        
                list = JsonConvert.DeserializeObject<List<RoomDto>>(Convert.ToString(response.Result));
                if (list.Count() > 0)
                {
                    list.All(t =>
                    {
                        var url = _configuration.GetSection("BaseUrl:WebUrl").Value;
                        //t.Image = "http://localhost:7001/" + t.Image;
                        t.Image = url + t.Image;
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

        #region Create Room
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> RoomCreate()
        {
            List<RoomTypeDto>? list = new();
            ResponseDto? response = await _roomTypeService.GetAllRoomTypesAsync();

            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<RoomTypeDto>>(Convert.ToString(response.Result));
            }
            /*List<RoomTypeDto> list = new List<RoomTypeDto>();*/

            /*foreach (var dr in list)
            {
                RoomTypeDto lst = new RoomTypeDto();
                lst.RoomTypeID = dr.RoomTypeID;
                lst.Name = dr.Name.ToString();
                list.Add(lst);
            }*/
            ViewBag.RoomTypeList = list;

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> RoomCreate(RoomDto model)
        {
            //For RoomType DropDown
            List<RoomTypeDto>? list = new();
            ResponseDto? response1 = await _roomTypeService.GetAllRoomTypesAsync();

            if (response1 != null && response1.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<RoomTypeDto>>(Convert.ToString(response1.Result));
            }

            ViewBag.RoomTypeList = list;


            if (ModelState.IsValid)
            {
                ResponseDto? response = await _roomService.CreateRoomAsync(model);

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Room created successfully";
                    return RedirectToAction("RoomIndex");
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }
            return View(model);
        }
        #endregion

        #region Delete Room
        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> RoomDelete(int roomId)
        {
            /*ResponseDto? response = await _roomService.GetRoomByIdAsync(roomId);

            if (response != null && response.IsSuccess)
            {
                RoomDto? model = JsonConvert.DeserializeObject<RoomDto>(Convert.ToString(response.Result));
                return View(model);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();*/

            ResponseDto? response = await _roomService.DeleteRoomAsync(roomId);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Room deleted successfully";
                return RedirectToAction("RoomIndex");
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return RedirectToAction("RoomIndex","Room");
        }

        [HttpPost]
        public async Task<IActionResult> RoomDelete(RoomDto roomDto)
        {
            ResponseDto? response = await _roomService.DeleteRoomAsync(roomDto.RoomID);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Room deleted successfully";
                return RedirectToAction("RoomIndex");
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(roomDto);
        }
        #endregion

        #region Update Room
        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> RoomEdit(int roomId)
        {
            //RoomType DropDown
            List<RoomTypeDto>? list = new();
            ResponseDto? response1 = await _roomTypeService.GetAllRoomTypesAsync();

            if (response1 != null && response1.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<RoomTypeDto>>(Convert.ToString(response1.Result));
            }

            ViewBag.RoomTypeList = list;

            //Update Data by ID
            ResponseDto? response = await _roomService.GetRoomByIdAsync(roomId);

            if (response != null && response.IsSuccess)
            {
                RoomDto? model = JsonConvert.DeserializeObject<RoomDto>(Convert.ToString(response.Result));
                return View(model);
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return NotFound();
        }   

        [HttpPost]
        public async Task<IActionResult> RoomEdit(RoomDto roomDto)
        {
            if (ModelState.IsValid)
            {
                ResponseDto? response = await _roomService.UpdateRoomAsync(roomDto);

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Room updated successfully";
                    return RedirectToAction("RoomIndex");
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }
            return View(roomDto);
        }
        #endregion

        public async Task<IActionResult> RoomFilter()
        {

            return View();
        }
    }
}
