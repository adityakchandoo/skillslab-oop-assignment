using MainLibrary.Entities.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.Entities
{
    public class UserTrainingEnrollment
    {
        public int UserTrainingEnrollmentId { get; set; }
        public string UserId { get; set; }
        public int TrainingId { get; set; }
        public DateTime ApplyDate { get; set; } = DateTime.Now;
        public DateTime? EnrolledDate { get; set; }
        public EnrollStatusType Status { get; set; }
    }
}
