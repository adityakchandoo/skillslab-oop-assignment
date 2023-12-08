using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DbModels
{
    public class TrainingPrerequisite
    {
        [Key]
        public int TrainingPrerequisiteId { get; set; }
        public int TrainingId { get; set; }
        public int PrerequisiteId { get; set; }
    }

}
