using MainLibrary.Entities.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime DOB { get; set; }
        public string NIC { get; set; }
        public string MobileNumber { get; set; }
        public StatusType Status { get; set; }
        public UserType UserType { get; set; }
    }    

}


