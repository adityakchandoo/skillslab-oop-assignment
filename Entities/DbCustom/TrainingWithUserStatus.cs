﻿using Entities.DbModels;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DbCustom
{
    public class TrainingWithUserStatus : Training
    { 
        public int ManagerApprovalStatus { get; set; }
        public int EnrollStatus { get; set; }
    }

}
