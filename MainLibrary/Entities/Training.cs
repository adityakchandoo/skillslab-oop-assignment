using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.Entities
{
    public class Training
    {
        public Int32 TrainingId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Int32 Treshhold { get; set; }
        public DateTime Deadline { get; set; }
        public string ManagerId { get; set; }
        public Int32? PreferedDepartmentId { get; set; }
    }
}
