﻿using Entities.DbModels;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DbCustom
{
    public class UserRoleAssigned
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int IsAssigned { get; set; }
    }
}
