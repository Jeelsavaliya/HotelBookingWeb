using HotelBookingWeb.Models;
using HotelBookingWeb.Service;
using HotelBookingWeb.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using HotelBookingWeb.Utility;
using System;
using System.IdentityModel.Tokens.Jwt;
using Stripe.Climate;
using Stripe.Checkout;
using System.Collections.Generic;
using SelectPdf;

namespace HotelBookingWeb.Controllers
{
    public class BookingRoomController : Controller
    {
        private readonly IBookingRoomService _bookingRoomService;
        private readonly IRoomService _roomService;
        private readonly IAuthService _authService;
        private readonly IMailService _mailService;
        private readonly IConfiguration _configuration;
        private ResponseDto _response;

        public BookingRoomController(IBookingRoomService bookingRoomService, IRoomService roomService, IAuthService authService, IMailService mailService, IConfiguration configuration)
        {
            _bookingRoomService = bookingRoomService;
            _roomService = roomService;
            _authService = authService;
            _mailService = mailService;
            _response = new ResponseDto();
            _configuration = configuration;
        }

        #region BookingRoom Index
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> BookingRoomIndex()
        {
            List<BookingRoomDto>? list = new();

            ResponseDto? response = await _bookingRoomService.GetAllBookingRoomAsync();

            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<BookingRoomDto>>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(list);
        }
        #endregion

        #region Create BookingRoom
        [HttpGet]
        public async Task<IActionResult> BookingRoom(int roomId)
        {

            ResponseDto? response = await _roomService.GetRoomByIdAsync(roomId);

            if (response != null && response.IsSuccess)
            {
                RoomDto? model = JsonConvert.DeserializeObject<RoomDto>(Convert.ToString(response.Result));
                if (model.Status == "Unoccupied")
                {
                    //Check user is Authenticate or not
                    if (User.Identity.IsAuthenticated)
                    {
                        return View();
                    }
                    return RedirectToAction("Login", "Auth");
                }
                else
                    return RedirectToAction("Rooms", "Home");
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> BookingRoom(BookingRoomDto model, int roomId)
        {

            if (ModelState.IsValid)
            {
                //Check user is Authenticate or not
                if (User.Identity.IsAuthenticated)
                {

                    ResponseDto? roomresponse = await _roomService.GetRoomByIdAsync(roomId);

                    //Get and check selected room is avaliable or not
                    if (roomresponse != null && roomresponse.IsSuccess)
                    {
                        RoomDto? models = JsonConvert.DeserializeObject<RoomDto>(Convert.ToString(roomresponse.Result));
                        //models.Status = "Occuppied";
                        models.Status = "Pre-Booked";

                        //For count total price for payment


                        var t1 = (model.CheckInDate).Day;
                        var t2 = (model.CheckOutDate).Day;
                        decimal datediff = t2 - t1;

                        datediff += 1;

                        model.TotalPrice = datediff * models.PricePerNight;

                        //For add UserId inn BookingRoom Table
                        string userId = "";
                        if (User.IsInRole(SD.RoleCustomer))
                        {
                            userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
                        }

                        model.UserId = userId;
                        model.Payment = "Pending";

                        ResponseDto? bookingresponse = await _bookingRoomService.CreateBookingRoomAsync(model);

                        //Check create booking is true or not
                        if (bookingresponse != null && bookingresponse.IsSuccess)
                        {
                            //TempData["success"] = "BookingRoom created successfully";


                            /*ResponseDto? emailresponse = await _mailService.SendMail(model, models);

                            //Check email response 
                            if (emailresponse != null && emailresponse.IsSuccess)
                            {*/
                            ResponseDto? updaetroom = await _roomService.UpdateRoomAsync(models);


                            //Check room is update or not
                            if (updaetroom != null && updaetroom.IsSuccess)
                            {
                                TempData["success"] = "Your Room is Booked, Now Pay your payment and get confirmed your book room....";
                                return RedirectToAction("MyBooking", "Home");
                            }
                            else
                            {
                                TempData["error"] = updaetroom?.Message;
                            }
                            return RedirectToAction("Rooms", "Home");
                            /* }
                             return View(model);*/
                        }
                        else
                        {
                            TempData["error"] = bookingresponse?.Message;
                        }
                        return View(model);
                    }
                    else
                    {
                        TempData["error"] = roomresponse?.Message;
                    }
                    return RedirectToAction("Index", "Home");
                }
                return RedirectToAction("Login", "Auth");
            }
            return View();
        }
        #endregion

        #region Delete BookingRoom
        [HttpGet]

        public async Task<IActionResult> BookingRoomDelete(int bookingRoomId)
        {/*
            ResponseDto? response = await _bookingRoomService.GetBookingRoomByIdAsync(roomId);

            if (response != null && response.IsSuccess)
            {
                BookingRoomDto? model = JsonConvert.DeserializeObject<BookingRoomDto>(Convert.ToString(response.Result));
                return View(model);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();*/

            ResponseDto? response = await _bookingRoomService.GetBookingRoomByIdAsync(bookingRoomId);


            if (response != null && response.IsSuccess)
            {
                BookingRoomDto bookingRoomDto = JsonConvert.DeserializeObject<BookingRoomDto>(Convert.ToString(response.Result));

                var roomId = bookingRoomDto.RoomID;
                ResponseDto? roomresponse = await _roomService.GetRoomByIdAsync(roomId);

                //Get and check selected room is avaliable or not
                if (roomresponse != null && roomresponse.IsSuccess)
                {
                    RoomDto? models = JsonConvert.DeserializeObject<RoomDto>(Convert.ToString(roomresponse.Result));
                    models.Status = "Unoccupied";

                    ResponseDto? updaetroom = await _roomService.UpdateRoomAsync(models);

                    //Check room is update or not
                    if (updaetroom != null && updaetroom.IsSuccess)
                    {
                        ResponseDto? deleteBookingRoom = await _bookingRoomService.DeleteBookingRoomAsync(bookingRoomId);
                        if (deleteBookingRoom != null && deleteBookingRoom.IsSuccess)
                        {
                            TempData["success"] = "BookingRoom deleted successfully";
                            return RedirectToAction("BookingRoomIndex");
                        }
                    }
                    else
                    {
                        TempData["error"] = updaetroom?.Message;
                    }
                    return RedirectToAction("BookingRoomIndex");

                }
                else
                {
                    TempData["error"] = roomresponse?.Message;
                }
                return RedirectToAction("BookingRoomIndex");
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return RedirectToAction("BookingRoomIndex");
        }

        /*[HttpPost]
        public async Task<IActionResult> BookingRoomDelete(BookingRoomDto roomDto)
        {
            ResponseDto? response = await _bookingRoomService.DeleteBookingRoomAsync(roomDto.BookingRoomID);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "BookingRoom deleted successfully";
                return RedirectToAction("BookingRoomIndex");
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(roomDto);
        }*/
        #endregion

        #region Update BookingRoom
        [HttpGet]
        /*[Authorize(Roles = "ADMIN")]*/
        public async Task<IActionResult> BookingRoomEdit(int bookingRoomId)
        {
            ResponseDto? response = await _bookingRoomService.GetBookingRoomByIdAsync(bookingRoomId);

            if (response != null && response.IsSuccess)
            {
                BookingRoomDto? model = JsonConvert.DeserializeObject<BookingRoomDto>(Convert.ToString(response.Result));
                return View(model);
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> BookingRoomEdit(BookingRoomDto bookingRoomDto, int roomId)
        {
            if (ModelState.IsValid)
            {
                ResponseDto? roomresponse = await _roomService.GetRoomByIdAsync(roomId);

                //Get and check selected room is avaliable or not
                if (roomresponse != null && roomresponse.IsSuccess)
                {
                    RoomDto? models = JsonConvert.DeserializeObject<RoomDto>(Convert.ToString(roomresponse.Result));

                    //For count total price for payment
                    var t1 = (bookingRoomDto.CheckInDate).Day;
                    var t2 = (bookingRoomDto.CheckOutDate).Day;
                    decimal datediff = t2 - t1;

                    datediff += 1;

                    bookingRoomDto.TotalPrice = datediff * models.PricePerNight;

                    ResponseDto? response = await _bookingRoomService.UpdateBookingRoomAsync(bookingRoomDto);

                    if (response != null && response.IsSuccess)
                    {
                        ResponseDto? emailresponse = await _mailService.SendMail(bookingRoomDto, models);

                        //Check email response 
                        if (emailresponse != null && emailresponse.IsSuccess)
                        {
                            TempData["success"] = "BookingRoom updated successfully";
                            return RedirectToAction("BookingRoomIndex");
                        }
                        return View(bookingRoomDto);
                    }
                    else
                    {
                        TempData["error"] = response?.Message;
                    }
                    return RedirectToAction("BookingRoomEdit");
                }
                else
                {
                    TempData["error"] = roomresponse?.Message;
                }
                return RedirectToAction("BookingRoomIndex");
            }
            return View(bookingRoomDto);
        }
        #endregion

        #region Check In

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> CheckIn(int bookingRoomId)
        {
            ResponseDto? bookingroomresponse = await _bookingRoomService.GetBookingRoomByIdAsync(bookingRoomId);
            if (bookingroomresponse != null && bookingroomresponse.IsSuccess)
            {
                BookingRoomDto? model = JsonConvert.DeserializeObject<BookingRoomDto>(Convert.ToString(bookingroomresponse.Result));
                var currentTime = DateTime.Now;
                model.CheckIn = "Check In" + " " + currentTime;

                ResponseDto? updatebookingroom = await _bookingRoomService.UpdateBookingRoomAsync(model);
                //Check room is update or not
                if (updatebookingroom != null && updatebookingroom.IsSuccess)
                {
                    TempData["success"] = "Chek-In Successfully";
                    return RedirectToAction("BookingRoomIndex");
                }
                else
                {
                    TempData["error"] = updatebookingroom?.Message;
                }
                return RedirectToAction("BookingRoomIndex");
            }
            else
            {
                TempData["error"] = bookingroomresponse?.Message;
            }
            return RedirectToAction("BookingRoomIndex");
        }
        #endregion

        #region Check Out

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> CheckOut(int bookingRoomId, int roomId)
        {
            ResponseDto? bookingroomresponse = await _bookingRoomService.GetBookingRoomByIdAsync(bookingRoomId);
            if (bookingroomresponse != null && bookingroomresponse.IsSuccess)
            {
                BookingRoomDto? model = JsonConvert.DeserializeObject<BookingRoomDto>(Convert.ToString(bookingroomresponse.Result));
                var currentTime = DateTime.Now;
                model.CheckOut = "Check Out" + " " + currentTime;

                ResponseDto? updatebookingroom = await _bookingRoomService.UpdateBookingRoomAsync(model);
                //Check room is update or not
                if (updatebookingroom != null && updatebookingroom.IsSuccess)
                {             
                    ResponseDto? roomresponse = await _roomService.GetRoomByIdAsync(roomId);

                    //Get and check selected room is avaliable or not
                    if (roomresponse != null && roomresponse.IsSuccess)
                    {
                        RoomDto? models = JsonConvert.DeserializeObject<RoomDto>(Convert.ToString(roomresponse.Result));
                        models.Status = "Unoccupied";

                        ResponseDto? updaetroom = await _roomService.UpdateRoomAsync(models);

                        //Check room is update or not
                        if (updaetroom != null && updaetroom.IsSuccess)
                        {
                            TempData["success"] = "Chek-Out Successfully";
                            return RedirectToAction("BookingRoomIndex");
                        }
                        else
                        {
                            TempData["error"] = updaetroom?.Message;
                        }
                        return RedirectToAction("BookingRoomIndex");

                    }
                    else
                    {
                        TempData["error"] = roomresponse?.Message;
                    }
                    return RedirectToAction("BookingRoomIndex");
                }
                else
                {
                    TempData["error"] = updatebookingroom?.Message;
                }
                return RedirectToAction("BookingRoomIndex");
            }
            else
            {
                TempData["error"] = bookingroomresponse?.Message;
            }
            return RedirectToAction("BookingRoomIndex");
        }
        #endregion

        #region Payment Satus
        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> PaymentStatus(int bookingRoomId)
        {
            ResponseDto? bookingroomresponse = await _bookingRoomService.GetBookingRoomByIdAsync(bookingRoomId);
            if (bookingroomresponse != null && bookingroomresponse.IsSuccess)
            {
                BookingRoomDto? model = JsonConvert.DeserializeObject<BookingRoomDto>(Convert.ToString(bookingroomresponse.Result));
                model.Payment = "Success";

                ResponseDto? updatebookingroom = await _bookingRoomService.UpdateBookingRoomAsync(model);
                //Check room is update or not
                if (updatebookingroom != null && updatebookingroom.IsSuccess)
                {

                    TempData["success"] = "Payment done successfully";
                    return RedirectToAction("BookingRoomIndex");
                }
            }
            return View();
        }


        #endregion

        #region Payment Gaytway
        public async Task<IActionResult> CheckoutPayment(int bookingroomId)
        {
            ResponseDto? response = await _bookingRoomService.GetBookingRoomByIdAsync(bookingroomId);

            if (response != null && response.IsSuccess)
            {
                BookingRoomDto bookingRoomDto = JsonConvert.DeserializeObject<BookingRoomDto>(Convert.ToString(response.Result));

                //Payment Gaytway using Stripe

                var domain = _configuration.GetSection("BaseUrl:SiteUrl").Value;
                

                var option = new SessionCreateOptions
                {
                    //SuccessUrl = domain + $"CheckoutPayment/BookingConfirmation",
                    SuccessUrl = domain + $"BookingRoom/PaymentSuccess?BookingRoomID=" +bookingRoomDto.BookingRoomID,
                    CancelUrl = domain + "Home/MyBooking",
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment"
                };


                var sessionListItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(bookingRoomDto.TotalPrice * 100),
                        Currency = "inr",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = bookingRoomDto.FirstName.ToString() + bookingRoomDto.LastName.ToString(),
                            /*Description = "Your Room Number: " + bookingRoomDto.RoomNumber.ToString() +*/
                        }
                    },
                    Quantity = 1
                };
                option.LineItems.Add(sessionListItem);


                var service = new SessionService();
                Session session = service.Create(option);

                Response.Headers.Add("Location", session.Url);


                return new StatusCodeResult(303);

               


                /*var roomId = bookingRoomDto.RoomID;

                ResponseDto? responseroom = await _roomService.GetRoomByIdAsync(roomId);

                if (responseroom != null && responseroom.IsSuccess)
                {
                    RoomDto roomDto = JsonConvert.DeserializeObject<RoomDto>(Convert.ToString(responseroom.Result));

                    roomDto.Status = "Occuppied";

                    //Upadte Payment Status
                    bookingRoomDto.Payment = "Success";

                    ResponseDto? updaetbookingroom = await _bookingRoomService.UpdateBookingRoomAsync(bookingRoomDto);
                    if (updaetbookingroom != null && updaetbookingroom.IsSuccess)
                    {
                        ResponseDto? emailresponse = await _mailService.SendMail(bookingRoomDto, roomDto);

                        //Check email response 
                        if (emailresponse != null && emailresponse.IsSuccess)
                        {
                            ResponseDto? updaetroom = await _roomService.UpdateRoomAsync(roomDto);


                            //Check room is update or not
                            if (updaetroom != null && updaetroom.IsSuccess)
                            {
                                TempData["success"] = "Your Payement Done Successfully & Your Room is Booked and Booking Detail will be sent on your mail.....";
                                return new StatusCodeResult(303);

                                *//*if(StatusCodeResult(303) == true)
                                {

                                }*//*
                                //return RedirectToAction("MyBooking", "Home");
                            }
                        }
                    }
                    else
                    {
                        TempData["error"] = updaetbookingroom?.Message;

                    }
                    return RedirectToAction("Rooms", "Home");
                }
                else
                {
                    TempData["error"] = "";
                }*/

                /*}
                else
                {
                    TempData["error"] = "Your Payement not done Successfully.....";
                    return RedirectToAction("MyBooking", "Home");
                }*/
            }
            return View();
        }
        #endregion

        #region Payment Success Status
        
        public async Task<IActionResult> PaymentSuccess(int bookingroomId)
        {
            ResponseDto? response = await _bookingRoomService.GetBookingRoomByIdAsync(bookingroomId);

            if (response != null && response.IsSuccess)
            {
                BookingRoomDto bookingRoomDto = JsonConvert.DeserializeObject<BookingRoomDto>(Convert.ToString(response.Result));

                var roomId = bookingRoomDto.RoomID;

                ResponseDto? responseroom = await _roomService.GetRoomByIdAsync(roomId);

                if (responseroom != null && responseroom.IsSuccess)
                {
                    RoomDto roomDto = JsonConvert.DeserializeObject<RoomDto>(Convert.ToString(responseroom.Result));

                    roomDto.Status = "Occuppied";

                    //Upadte Payment Status
                    bookingRoomDto.Payment = "Success";

                    ResponseDto? updaetbookingroom = await _bookingRoomService.UpdateBookingRoomAsync(bookingRoomDto);
                    if (updaetbookingroom != null && updaetbookingroom.IsSuccess)
                    {
                        ResponseDto? emailresponse = await _mailService.SendMail(bookingRoomDto, roomDto);

                        //Check email response 
                        if (emailresponse != null && emailresponse.IsSuccess)
                        {
                            ResponseDto? updaetroom = await _roomService.UpdateRoomAsync(roomDto);


                            //Check room is update or not
                            if (updaetroom != null && updaetroom.IsSuccess)
                            {
                                TempData["success"] = "Your Payement Done Successfully & Your Room is Booked and Booking Detail will be sent on your mail.....";
                                return RedirectToAction("MyBooking", "Home");
                                //return View();
                            }
                        }
                    }
                    else
                    {
                        TempData["error"] = updaetbookingroom?.Message;

                    }
                    return RedirectToAction("Rooms", "Home");
                }
                else
                {
                    TempData["error"] = "";
                }
            }
            return View();
        }
        #endregion

        #region Generate PDF
        public async Task<IActionResult> GeneratePDF(int bookingRoomId)
        {
            ResponseDto? response = await _bookingRoomService.GetBookingRoomByIdAsync(bookingRoomId);

            if (response != null && response.IsSuccess)
            {
                BookingRoomDto bookingRoomDto = JsonConvert.DeserializeObject<BookingRoomDto>(Convert.ToString(response.Result));

                int roomId = bookingRoomDto.RoomID;

                ResponseDto? responseroom = await _roomService.GetRoomByIdAsync(roomId);

                if (responseroom != null && responseroom.IsSuccess)
                {
                    RoomDto roomDto = JsonConvert.DeserializeObject<RoomDto>(Convert.ToString(responseroom.Result));


                    var stringdata = $@"<html>
                                <head>
                                    <style>
                                        body {{
                                            font-family: Arial, sans-serif;
                                            margin: 0;
                                            padding: 0;
                                            background-color: #f4f4f9;
                                            color: #333;
                                            backgroung-height: 100%;
                                            
                                        }}
                                        .container {{
                                            width: 100%;
                                            max-width: 1000px;
                                            margin: auto;
                                            background-color: #fff;
                                            padding: 20px;
                                            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                                        }}
                                        .header {{
                                            background-color: black;
                                            color: white;
                                            padding: 10px 0;
                                            text-align: center;
                                        }}
                                        .content {{
                                            padding: 20px;
                                        }}
                                        .content p {{
                                            margin: 10px 0;

                                        }}
                                        .button {{
                                            display: inline-block;
                                            padding: 10px 20px;
                                            margin-top: 20px;
                                            background-color: #007bff;
                                            color: #fff;
                                            text-decoration: none;
                                            border-radius: 5px;
                                        }}
                                        .footer {{
                                            text-align: center;
                                            margin-top: 20px;
                                            font-size: 12px;
                                            color: #aaa;
                                        }}
                                    </style>
                                </head>
                                <body>
                                    <div class='container'>
                                        <div class='header'>
                                            <h1>Booking Confirmation</h1>
                                        </div>
                                        <div class='content'>
                                            <p>Thank you for booking a room in our hotel.</p>
                                            <p>Your room has been booked successfully. Below are your booking details:</p><br/>
                                            <p><strong>Your Room Detail:</p>    
                                            <p><strong>Your Room Number : {roomDto.RoomNumber}</p>
                                            <p><strong>Your Room Capacity : {roomDto.Capacity}</p><br/>
                                            <p><strong>Booking Person Detail:</p>
                                            <p><strong>First Name:</strong> {bookingRoomDto.FirstName} </p>
                                            <p><strong>Last Name:</strong> {bookingRoomDto.LastName}</p>
                                            <p><strong>Address:</strong> {bookingRoomDto.Address}</p>
                                            <p><strong>Check-In Date:</strong> {bookingRoomDto.CheckInDate.ToString("dd-MM-yyyy")}</p>
                                            <p><strong>Check-Out Date:</strong> {bookingRoomDto.CheckOutDate.ToString("dd-MM-yyyy")}</p>
                                            <p><strong>Room Price:</strong> {bookingRoomDto.TotalPrice}</p>
                                            <p><strong>Payment Status:</strong> Payment Done Successfully</p>
                                        </div>
                                    </div>
                                </body>
                                </html>";


                    HtmlToPdf converter = new HtmlToPdf();
                    //html = html.Replace("start", "<").Replace("end", ">");
                    PdfDocument doc = converter.ConvertHtmlString(stringdata);
                    // doc.Save($@"{AppDomain.CurrentDomain.BaseDirectory}\url.pdf");
                    /*byte[] pdfFile = doc.Save();
                    doc.Close();
                    return File(pdfFile, "application/pdf");*/

                    // save pdf document
                    byte[] pdf = doc.Save();

                    // close pdf document
                    doc.Close();

                    // return resulted pdf document
                    FileResult fileResult = new FileContentResult(pdf, "application/pdf");
                    fileResult.FileDownloadName = "Document.pdf";
                    return fileResult;
                }
            }
            return View();
            /*RedirectToAction("Index","Home");*/
        }
        #endregion
    }
}

