using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.DTO
{
    public class UserLoginFormDTO
    {
        [Required(ErrorMessage = "Enter UserId")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Enter UserId")]
        public string Password { get; set; }
    }
}
