using System.ComponentModel.DataAnnotations;

namespace HotelBookingWeb.Models
{
    public class ForgotPasswordDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public  string NewPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
