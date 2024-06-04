using HotelBookingWeb.Authentication;
using HotelBookingWeb.Models;
using HotelBookingWeb.Service;
using HotelBookingWeb.Service.IService;
using HotelBookingWeb.Utility;
using MailKit.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Razorpay.Api;
using Stripe;
using Stripe.Checkout;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Xml.Linq;

namespace HotelBookingWeb.Controllers
{

	/* [CheckAccess]*/

	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IRoomService _roomService;
		private readonly IBookingRoomService _bookingRoomService;

		public HomeController(ILogger<HomeController> logger, IRoomService roomService, IBookingRoomService bookingRoomService)
		{
			_logger = logger;
			_roomService = roomService;
			_bookingRoomService = bookingRoomService;
		}


		public IActionResult Index()
		{
			return View();
		}

		public async Task<IActionResult> Rooms()
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

		public IActionResult AboutUs()
		{
			return View();
		}

		public IActionResult RoomDetails()
		{

			return View();
		}

		/*public async Task<IActionResult> RoomDeetails(int roomId)
        {
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
            return View();
        }*/

		public IActionResult Blogs()
		{
			return View();
		}

		public IActionResult Contact()
		{
			return View();
		}

		public async Task<IActionResult> MyBooking()
		{
			string userId = "";
			if (User.IsInRole(SD.RoleCustomer))
			{
				userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
			}

			List<BookingRoomDto>? list = new();

			ResponseDto? response = await _bookingRoomService.GetBookingRoomByUserIdAsync(userId);

			if (response != null && response.IsSuccess)
			{
				list = JsonConvert.DeserializeObject<List<BookingRoomDto>>(Convert.ToString(response.Result));

				foreach (var model in list)
				{
					//BookingRoomDto? model = JsonConvert.DeserializeObject<BookingRoomDto>(Convert.ToString(booking));
					if (model != null)
					{
						ResponseDto? responseroom = await _roomService.GetRoomByIdAsync(model.RoomID);
						if (response != null && response.IsSuccess)
						{
							RoomDto? modelroom = JsonConvert.DeserializeObject<RoomDto>(Convert.ToString(responseroom.Result));
							model.RoomNumber = modelroom.RoomNumber;
							model.Capacity = modelroom.Capacity;
						}
						//return View(model);
					}

				}

				return View(list);
			}
			else
			{
				TempData["error"] = response?.Message;
			}
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
