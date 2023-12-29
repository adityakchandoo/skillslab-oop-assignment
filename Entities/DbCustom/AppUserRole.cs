using Entities.DbModels;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DbCustom
{
    public class AppUserRole
    {
        [Required]
        public int RoleId { get; set; }
        [Required]
        public string RoleName { get; set; }
    }
}
