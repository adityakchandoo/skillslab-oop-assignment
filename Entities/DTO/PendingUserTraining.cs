using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO
{
    public class PendingUserTraining
    {
        public string UserId { get; set; }
        public int TrainingId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string TrainingName { get; set; }
        public DateTime TrainingDeadline { get; set; }

    }
}
