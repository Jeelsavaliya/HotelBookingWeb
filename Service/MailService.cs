using HotelBookingWeb.Models;
using HotelBookingWeb.Service.IService;
using HotelBookingWeb.Utility;
using Microsoft.Extensions.Options;
using Stripe;
using System.Net;
using System.Net.Mail;

namespace HotelBookingWeb.Service
{
    public class MailService : IMailService
    {
        /*private readonly MailSettings _mailSettings;
        public MailService(IOptions<MailSettings> mailSettingsOptions)
        {
            _mailSettings = mailSettingsOptions.Value;
        }*/
       /* private readonly IBaseService _baseService;
        public MailService(IBaseService baseService)
        {
            _baseService = baseService;
        }*/

        public async Task<ResponseDto?> SendMail(BookingRoomDto bookingRoomDto, RoomDto roomDto)
        {

            try
            {
                /*var t1 = (bookingRoomDto.CheckInDate).Day;
                var t2 = (bookingRoomDto.CheckOutDate).Day;
                decimal datediff = t2 - t1;

                //TimeSpan t1 = TimeSpan.Parse(bookingRoomDto.CheckInDate);
                //TimeSpan datediff = Convert.ToDecimal(bookingRoomDto.CheckOutDate - bookingRoomDto+-+.CheckInDate);
                if (datediff == 0)
                {
                    datediff = 1;
                }*/

                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress("jeelsavaliya007@gmail.com");

                message.To.Add(new MailAddress(bookingRoomDto.Email));

               /* foreach (var address in bookingRoomDto.Email.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    message.To.Add(new MailAddress(address));
                }*/

                message.Subject = "Booking Your Room";
                message.IsBodyHtml = true; //to make message body as html
                message.Body = /*"Thank you for Booking Room in our Hotel.....<br/>" +
                           "Your Room Booked successfully & Customer Details is here <br/>" +
                           "First Name : " + bookingRoomDto.FirstName + "Last Name : " + bookingRoomDto.LastName + "<br/>" +
                           "Address : " + bookingRoomDto.Address + "<br/>" +
                           "CheckInDate : " + bookingRoomDto.CheckInDate + "CheckOutDate : " + bookingRoomDto.CheckOutDate;*/

                                $@"
                                <html>
                                <head>
                                    <style>
                                        body {{
                                            font-family: Arial, sans-serif;
                                            margin: 0;
                                            padding: 0;
                                            background-color: #f4f4f9;
                                            color: #333;
                                        }}
                                        .container {{
                                            width: 100%;
                                            max-width: 600px;
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
                                            <p><strong>Your Room Number : {roomDto.RoomNumber }</p>
                                            <p><strong>Your Room Capacity : {roomDto.Capacity}</p><br/>
                                            <p><strong>Booking Person Detail:</p>
                                            <p><strong>First Name:</strong> {bookingRoomDto.FirstName} </p>
                                            <p><strong>Last Name:</strong> {bookingRoomDto.LastName}</p>
                                            <p><strong>Address:</strong> {bookingRoomDto.Address}</p>
                                            <p><strong>Check-In Date:</strong> {bookingRoomDto.CheckInDate}</p>
                                            <p><strong>Check-Out Date:</strong> {bookingRoomDto.CheckOutDate}</p>
                                            <p><strong>Room Price:</strong> {bookingRoomDto.TotalPrice}</p>
                                            <p><strong>Payment Status:</strong> Payment Done Successfully</p>
                                        </div>
                                    </div>
                                </body>
                                </html>";


                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com"; //for gmail host
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("jeelsavaliya007@gmail.com", "jkqw rnhk kdfv crwh"); //password is create from gmail security
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);

                var dto = new ResponseDto
                {
                    Message = "Mail send successfully",
                    IsSuccess = true
                };
                return dto;

            }
            catch (Exception ex)
            {
                var dto = new ResponseDto
                {
                    Message = ex.Message.ToString(),
                    IsSuccess = false
                };
                return dto;
            }


        }
    }
}
