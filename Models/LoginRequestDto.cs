using System.ComponentModel.DataAnnotations;

namespace HotelBookingWeb.Models
{
    public class LoginRequestDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
