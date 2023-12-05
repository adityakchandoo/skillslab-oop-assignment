using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DbModels
{
    public class Prerequisite
    {
        [Key]
        public int PrerequisiteId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
