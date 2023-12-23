using Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DbModels
{
    public class Training
    {
        [Key]
        public int TrainingId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MaxSeat { get; set; }
        public DateTime Deadline { get; set; }
        public int? PreferedDepartmentId { get; set; }
        public IsActive IsActive { get; set; } = IsActive.Yes;

    }

}
