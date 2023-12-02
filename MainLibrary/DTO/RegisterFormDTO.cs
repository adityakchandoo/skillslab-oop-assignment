using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using MainLibrary.Helpers;

namespace MainLibrary.DTO
{
    public class RegisterFormDTO
    {
        [Required(ErrorMessage = "Enter UserId")]
        public string UserId { get; set; }
        [Required(ErrorMessage = "Enter FirstName")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Enter LastName")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Enter Date Of Birth")]
        public DateTime DOB { get; set; }
        [Required(ErrorMessage = "Enter Email")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Enter NIC"), RegularExpression("^.{1}[0-9]{6}.{7}$", ErrorMessage = "Invalid NIC")]
        public string NIC { get; set; }
        [Required(ErrorMessage = "Enter MobileNumber")]
        public string MobileNumber { get; set; }
        [Required(ErrorMessage = "Enter Password")]
        public string Pass1 { get; set; }
        [Required(ErrorMessage = "Enter Comfirm Password")]
        [Compare("Pass1", ErrorMessage = "Confirm Pass Not Same")]
        public string Pass2 { get; set; }
    }
}
