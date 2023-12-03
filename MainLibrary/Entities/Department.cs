using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.Entities
{
    public class Department
    {
        public virtual int DepartmentId { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
    }
}
