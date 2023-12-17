using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Entities.FormDTO
{
    public class RegisterFormDTO
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
        public string Email { get; set; }
        public string NIC { get; set; }
        public string MobileNumber { get; set; }
        public string Pass1 { get; set; }
        public string Pass2 { get; set; }
        public int ManagerId { get; set; }
        public int DepartmentId { get; set; }
    }
}
