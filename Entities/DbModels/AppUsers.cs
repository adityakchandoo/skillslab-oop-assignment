using Entities.Enums;
using Entities.Other;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DbModels
{
    public class AppUser
    {
        //[Key] Because of not autoincrementing
        [Key]
        [NotIdentity]
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime DOB { get; set; }
        public string NIC { get; set; }
        public string MobileNumber { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public string ManagerId { get; set; } = null;
        public int? DepartmentId { get; set; } = null;
        public UserStatusEnum Status { get; set; }
        public UserRoleEnum Role { get; set; }
    }

}
