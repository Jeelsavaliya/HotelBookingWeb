using HotelBookingWeb.Models;
namespace HotelBookingWeb.Service.IService
{
    public interface IMailService
    {
         /*bool SendMail(MailData mailData);*/
         Task<ResponseDto?> SendMail(BookingRoomDto bookingRoomDto, RoomDto roomDto);
    }
}


