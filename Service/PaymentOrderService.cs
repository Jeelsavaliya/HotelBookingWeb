/*using HotelBookingWeb.Models;
using HotelBookingWeb.Service.IService;
using HotelBookingWeb.Utility;

namespace HotelBookingWeb.Service
{
    public class PaymentOrderService : IPaymentOrderService
    {
        private readonly IBaseService _baseService;
        public PaymentOrderService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto?> CreateStripeSession(StripeRequestDto stripeRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = stripeRequestDto,
                Url = SD.PaymentOrderAPIBase + "/api/PaymentOrder/CreateStripeSession"
            }); 
        }
    }
}
*/