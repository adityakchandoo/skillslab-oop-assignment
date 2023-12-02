using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.Entities
{
    public class Training
    {
        public int TrainingId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Treshhold { get; set; }
        public DateTime Deadline { get; set; }
        public string ManagerId { get; set; }
        public int? PreferedDepartmentId { get; set; }
    }
}
