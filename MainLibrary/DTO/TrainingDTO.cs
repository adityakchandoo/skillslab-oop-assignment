﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.DTO
{
    public class TrainingDTO
    {
        public int TrainingId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Threshold { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Deadline { get; set; }
        public string ManagerId { get; set; }
        public int PriorityDepartmentId { get; set; }
        public int[] Prerequisites { get; set; }

    }
}
