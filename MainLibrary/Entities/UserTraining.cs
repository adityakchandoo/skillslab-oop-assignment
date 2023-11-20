using MainLibrary.Entities.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.Entities
{
    public class UserTraining
    {
        public User User { get; set; }
        public Training Training { get; set; }
        public StatusType Status { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
