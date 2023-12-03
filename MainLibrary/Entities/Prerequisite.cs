using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.Entities
{
    public class Prerequisite
    {
        public virtual int PrerequisiteId { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }

    }
}
