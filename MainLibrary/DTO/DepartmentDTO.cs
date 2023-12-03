using MainLibrary.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.DTO
{
    public class DepartmentDTO : Department
    {
        public override int DepartmentId { get; set; }
        [Required]
        public override string Name { get; set; }
        [Required]
        public override string Description { get; set; }
    }
}
