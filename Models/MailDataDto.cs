using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBookingWeb.Models
{
    public class MailDataDto
    {
        public int Id { get; set; }
        
        public string EmailToID { get; set; }
        
        public string EmailToName { get; set; }
        
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
    }
}
