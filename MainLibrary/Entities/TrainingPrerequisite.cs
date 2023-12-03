using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.Entities
{
    public class TrainingPrerequisite
    {
        public int TrainingPrerequisiteId { get; set; }
        public int TrainingId { get; set; }
        public int PrerequisiteId { get; set; }
    }
}
