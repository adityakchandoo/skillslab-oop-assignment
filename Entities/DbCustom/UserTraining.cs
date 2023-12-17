using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DbCustom
{
    public class UserTraining
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int TrainingId { get; set; }
        public string Name { get; set; }
        public string TrainingName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public DateTime TrainingDeadline { get; set; }

    }
}
