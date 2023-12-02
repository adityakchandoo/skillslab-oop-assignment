using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.Entities
{
    public class TrainingDetails : Training
    {
        public string ManagerName { get; set; }
        public string DepartmentName { get; set; }
    }
}
