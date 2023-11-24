using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MainLibrary.DTO
{
    public class RegisterFormDTO
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime DOB { get; set; }
        public string NIC { get; set; }
        public string MobileNumber { get; set; }
    }
}
