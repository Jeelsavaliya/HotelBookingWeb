using Microsoft.AspNetCore.Identity;

namespace HotelBookingWeb.Models
{
    public class ApplicationUserDto : IdentityUser
    {
        public string Name { get; set; }
    }
}
