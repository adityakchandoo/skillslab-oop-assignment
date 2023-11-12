using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.Entities
{
    public class User
    {
        public User(int id, string firstName, string lastName, string email, string dOB, string nIC, string mobileNumber, UserType userType)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            DOB = dOB;
            NIC = nIC;
            MobileNumber = mobileNumber;
            UserType = userType;
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string DOB { get; set; }
        public string NIC { get; set; }
        public string MobileNumber { get; set; }
        public UserType UserType { get; set; }
    }    

}


