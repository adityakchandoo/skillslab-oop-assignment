using Entities.DbModels;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DbCustom
{
    public class AppUserDetails : AppUser
    {
        public UserRoleEnum Role { get; set; }
        public string RoleName { get; set; }
        public int ManagerId { get; set; }
    }
}
